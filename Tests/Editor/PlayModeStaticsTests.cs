// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Buck;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

/// <summary>
/// Checks that every Play session starts from the same state whether or not the editor reloads the
/// scripting domain (Enter Play Mode Settings). The fixtures without domain reload run two Play
/// sessions: the first dirties the statics of Singleton&lt;T&gt; and SoftSingleton&lt;T&gt; and the runtime
/// state of a GameEvent, a BoolVariable, a clamped IntVariable and a RuntimeSet the way a game would,
/// and the second must find all of it reset. The fixture with domain reload checks that a session
/// starts exactly as it did before the play-session resets existed. Every fixture restores the
/// project's Enter Play Mode Settings afterwards.
/// </summary>
public abstract class PlayModeStaticsTestBase
{
    const string k_assetPrefix = "PlayModeStaticsTests ";
    const string k_settingsKey = "Buck.PlayModeStaticsTests.";
    protected const BindingFlags k_staticFlags = BindingFlags.NonPublic | BindingFlags.Static;
    protected const BindingFlags k_instanceFlags = BindingFlags.NonPublic | BindingFlags.Instance;

    protected static readonly Type k_singleton = typeof(Singleton<PlayModeStaticsTestSingleton>);
    protected static readonly Type k_softSingleton = typeof(SoftSingleton<PlayModeStaticsTestSoftSingleton>);

    protected abstract EnterPlayModeOptions Options { get; }

    protected GameEvent m_restartEvent;
    protected BoolVariable m_variable;
    protected IntVariable m_number;
    protected PlayModeStaticsTestRuntimeSet m_set;

    // Failures are collected rather than thrown so that the editor always leaves Play mode at the
    // end of a test, whatever went wrong inside a session.
    readonly List<string> m_failures = new();

    [SetUp]
    public void SetUp()
    {
        // SessionState survives the domain reload that one of the fixtures goes through.
        SessionState.SetBool(k_settingsKey + "Enabled", EditorSettings.enterPlayModeOptionsEnabled);
        SessionState.SetInt(k_settingsKey + "Options", (int)EditorSettings.enterPlayModeOptions);
        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions = Options;

        // In-memory assets that stay loaded across Play sessions, like project assets do.
        m_restartEvent = CreateAsset<GameEvent>("Restart Event");
        m_variable = CreateAsset<BoolVariable>("Variable");
        m_number = CreateAsset<IntVariable>("Number");
        m_set = CreateAsset<PlayModeStaticsTestRuntimeSet>("Set");

        // The variable resets on this event, so it registers a listener on it whenever it is enabled.
        GetField<List<GameEvent>>(typeof(BaseVariable<bool>), m_variable, "m_restartEvents").Add(m_restartEvent);

        // A default above the clamp. OnEnable resets the value and then clamps it, so every session
        // has to start at 100, never at 150.
        typeof(BaseVariable<double>).GetField("m_defaultValue", k_instanceFlags).SetValue(m_number, 150.0);
        typeof(NumberVariable).GetField("m_clampToAMax", k_instanceFlags).SetValue(m_number, true);
        typeof(NumberVariable).GetField("m_clampMax", k_instanceFlags).SetValue(m_number, new NumberReference(100f));

        // CreateInstance ran OnEnable before those fields were set; run it again, as loading the
        // assets would.
        InvokeOnEnable(m_variable);
        InvokeOnEnable(m_number);

        m_failures.Clear();
    }

    [TearDown]
    public void TearDown()
    {
        // Found by name: a domain reload drops this fixture's fields.
        foreach (var asset in Resources.FindObjectsOfTypeAll<GameEvent>())
            if (asset.name.StartsWith(k_assetPrefix, StringComparison.Ordinal))
                Object.DestroyImmediate(asset);

        EditorSettings.enterPlayModeOptions = (EnterPlayModeOptions)SessionState.GetInt(k_settingsKey + "Options", 0);
        EditorSettings.enterPlayModeOptionsEnabled = SessionState.GetBool(k_settingsKey + "Enabled", false);
    }

    // Two Play sessions without domain reload: the first uses everything the way a game would, the
    // second must find all of it reset.
    protected IEnumerator RunTwoSessions()
    {
        yield return new EnterPlayMode(expectDomainReload: false);

        Check(PlayModeStaticsTestSingleton.Instance != null, "session 1: Singleton<T>.Instance is created");
        new GameObject("PlayModeStaticsTests").AddComponent<PlayModeStaticsTestSoftSingleton>();
        Check(PlayModeStaticsTestSoftSingleton.Instance != null, "session 1: SoftSingleton<T>.Instance is set by Awake");
        Check(ListenerCount(m_restartEvent) == 1, "session 1: the variable's restart listener is registered once");
        Check(m_number.Value == 100, "session 1: the number starts at its clamped default");
        m_restartEvent.RegisterListener(new GameEventListenerReference
        {
            Event = m_restartEvent,
            EventListener = m_restartEvent,
            OnEventRaisedDelegate = () => { }
        });
        m_variable.Value = true;
        m_number.Value = 42;
        m_set.Add(new GameObject("PlayModeStaticsTests item"));

        yield return new ExitPlayMode();

        // Between sessions: without a domain reload all of it is still there.
        Check((bool)GetStatic(k_singleton, "m_AppIsQuitting"), "between sessions: Singleton<T> is still flagged as quitting");
        Check(m_variable.Value, "between sessions: the variable keeps the previous session's value");
        Check(m_number.Value == 42, "between sessions: the number keeps the previous session's value");
        Check(ListenerCount(m_restartEvent) == 2, "between sessions: the event keeps the previous session's listeners");
        Check(m_set.Items.Count == 1, "between sessions: the runtime set keeps the previous session's item");

        // Session two: the play-session resets ran at SubsystemRegistration.
        yield return new EnterPlayMode(expectDomainReload: false);

        Check(!(bool)GetStatic(k_singleton, "m_ShuttingDown"), "session 2: Singleton<T> is not shutting down");
        Check(!(bool)GetStatic(k_singleton, "m_AppIsQuitting"), "session 2: Singleton<T> is not quitting");
        Check(GetStatic(k_singleton, "m_Instance") == null, "session 2: Singleton<T> dropped the previous session's instance");
        Check(PlayModeStaticsTestSingleton.Instance != null, "session 2: Singleton<T>.Instance is created again");
        Check(GetStatic(k_softSingleton, "m_Instance") == null, "session 2: SoftSingleton<T> dropped the previous session's instance");
        Check(!m_variable.Value, "session 2: the variable is back at its default value");
        Check(m_number.Value == 100, "session 2: the number is back at its clamped default");
        Check(ListenerCount(m_restartEvent) == 1, "session 2: only the variable's restart listener is registered");
        Check(RestartListenerCount(m_variable) == 1, "session 2: the variable holds one restart listener reference");
        Check(m_set.Items.Count == 0, "session 2: the runtime set is empty");

        yield return new ExitPlayMode();

        AssertNoFailures();
    }

    protected void Check(bool condition, string expectation)
    {
        if (!condition)
            m_failures.Add(expectation);
    }

    protected void AssertNoFailures()
        => Assert.IsEmpty(m_failures, string.Join("\n", m_failures));

    static T CreateAsset<T>(string name) where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        asset.name = k_assetPrefix + name;
        asset.hideFlags = HideFlags.HideAndDontSave;
        return asset;
    }

    protected static T FindAsset<T>(string name) where T : Object
        => Array.Find(Resources.FindObjectsOfTypeAll<T>(), asset => asset.name == k_assetPrefix + name);

    static void InvokeOnEnable(BaseVariable variable)
        => variable.GetType().GetMethod("OnEnable", k_instanceFlags).Invoke(variable, null);

    protected static object GetStatic(Type type, string field)
        => type.GetField(field, k_staticFlags).GetValue(null);

    static T GetField<T>(Type type, object instance, string field)
        => (T)type.GetField(field, k_instanceFlags).GetValue(instance);

    protected static int ListenerCount(GameEvent gameEvent)
        => GetField<ICollection>(typeof(GameEvent), gameEvent, "m_eventListenerReferences").Count;

    static int RestartListenerCount(BoolVariable variable)
        => GetField<ICollection>(typeof(BaseVariable<bool>), variable, "m_restartEventListenerReferences").Count;
}

public class PlayModeStaticsTests : PlayModeStaticsTestBase
{
    protected override EnterPlayModeOptions Options => EnterPlayModeOptions.DisableDomainReload;

    [UnityTest]
    public IEnumerator StateIsResetAtTheStartOfEveryPlaySessionWithoutDomainReload()
        => RunTwoSessions();
}

public class PlayModeStaticsWithoutSceneReloadTests : PlayModeStaticsTestBase
{
    protected override EnterPlayModeOptions Options
        => EnterPlayModeOptions.DisableDomainReload | EnterPlayModeOptions.DisableSceneReload;

    [UnityTest]
    public IEnumerator StateIsResetAtTheStartOfEveryPlaySessionWithoutDomainOrSceneReload()
        => RunTwoSessions();
}

public class PlayModeStaticsWithDomainReloadTests : PlayModeStaticsTestBase
{
    protected override EnterPlayModeOptions Options => EnterPlayModeOptions.None;

    // Projects that keep the domain reload on must see no change: the play-session resets run after
    // the reload's own OnEnable and have to agree with it.
    [UnityTest]
    public IEnumerator SessionStartsAsBeforeWithDomainReload()
    {
        m_number.Value = 42; // edit-mode state that the reload has to discard

        yield return new EnterPlayMode();

        // The domain reload dropped this fixture's fields; find the assets again.
        var number = FindAsset<IntVariable>("Number");
        var variable = FindAsset<BoolVariable>("Variable");
        var restartEvent = FindAsset<GameEvent>("Restart Event");
        Check(number != null && number.Value == 100, "the number starts at its clamped default");
        Check(variable != null && !variable.Value, "the variable starts at its default value");
        Check(restartEvent != null && ListenerCount(restartEvent) == 1, "the variable's restart listener is registered once");
        Check(PlayModeStaticsTestSingleton.Instance != null, "Singleton<T>.Instance is created");
        Check(!(bool)GetStatic(k_singleton, "m_ShuttingDown"), "Singleton<T> is not shutting down");

        yield return new ExitPlayMode();

        AssertNoFailures();
    }
}

public class PlayModeStaticsTestSingleton : Singleton<PlayModeStaticsTestSingleton> { }

public class PlayModeStaticsTestSoftSingleton : SoftSingleton<PlayModeStaticsTestSoftSingleton> { }

public class PlayModeStaticsTestRuntimeSet : RuntimeSet<GameObject> { }
