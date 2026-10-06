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

/// <summary>
/// Checks that PlayModeStatics gives every Play session a clean slate when the editor does not
/// reload the scripting domain (Enter Play Mode Settings). The test runs two Play sessions with
/// domain reload disabled: the first dirties the statics of Singleton&lt;T&gt; and SoftSingleton&lt;T&gt;
/// and the runtime state of a GameEvent, a BoolVariable and a RuntimeSet the way a game would,
/// and the second must find all of it reset. The project's Enter Play Mode Settings are restored
/// afterwards. With the domain reload on there is nothing to test: the reload itself resets it all.
/// </summary>
public class PlayModeStaticsTests
{
    const BindingFlags k_staticFlags = BindingFlags.NonPublic | BindingFlags.Static;
    const BindingFlags k_instanceFlags = BindingFlags.NonPublic | BindingFlags.Instance;

    static readonly Type k_singleton = typeof(Singleton<PlayModeStaticsTestSingleton>);
    static readonly Type k_softSingleton = typeof(SoftSingleton<PlayModeStaticsTestSoftSingleton>);

    bool m_optionsEnabled;
    EnterPlayModeOptions m_options;

    GameEvent m_restartEvent;
    BoolVariable m_variable;
    PlayModeStaticsTestRuntimeSet m_set;

    // Failures are collected rather than thrown so that the editor always leaves Play mode at the
    // end of the test, whatever went wrong inside a session.
    readonly List<string> m_failures = new();

    [SetUp]
    public void SetUp()
    {
        m_optionsEnabled = EditorSettings.enterPlayModeOptionsEnabled;
        m_options = EditorSettings.enterPlayModeOptions;
        EditorSettings.enterPlayModeOptionsEnabled = true;
        EditorSettings.enterPlayModeOptions = EnterPlayModeOptions.DisableDomainReload;

        // In-memory assets that stay loaded across the Play sessions, like project assets do.
        m_restartEvent = ScriptableObject.CreateInstance<GameEvent>();
        m_restartEvent.hideFlags = HideFlags.HideAndDontSave;
        m_variable = ScriptableObject.CreateInstance<BoolVariable>();
        m_variable.hideFlags = HideFlags.HideAndDontSave;
        m_set = ScriptableObject.CreateInstance<PlayModeStaticsTestRuntimeSet>();
        m_set.hideFlags = HideFlags.HideAndDontSave;

        // Resetting on this event makes the variable register a listener on it at the start of
        // every Play session (OnEnable already ran, with no restart events, in CreateInstance).
        GetInstanceField<List<GameEvent>>(typeof(BaseVariable<bool>), m_variable, "m_restartEvents").Add(m_restartEvent);

        m_failures.Clear();
    }

    [TearDown]
    public void TearDown()
    {
        UnityEngine.Object.DestroyImmediate(m_set);
        UnityEngine.Object.DestroyImmediate(m_variable);
        UnityEngine.Object.DestroyImmediate(m_restartEvent);

        EditorSettings.enterPlayModeOptions = m_options;
        EditorSettings.enterPlayModeOptionsEnabled = m_optionsEnabled;
    }

    [UnityTest]
    public IEnumerator StateIsResetAtTheStartOfEveryPlaySessionWithoutDomainReload()
    {
        // Session one: use everything the way a game would.
        yield return new EnterPlayMode(expectDomainReload: false);

        Check(PlayModeStaticsTestSingleton.Instance != null, "session 1: Singleton<T>.Instance is created");
        new GameObject("PlayModeStaticsTests").AddComponent<PlayModeStaticsTestSoftSingleton>();
        Check(PlayModeStaticsTestSoftSingleton.Instance != null, "session 1: SoftSingleton<T>.Instance is set by Awake");
        Check(ListenerCount(m_restartEvent) == 1, "session 1: the variable registered its restart listener at session start");
        m_restartEvent.RegisterListener(new GameEventListenerReference
        {
            Event = m_restartEvent,
            EventListener = m_restartEvent,
            OnEventRaisedDelegate = () => { }
        });
        m_variable.Value = true;
        m_set.Add(new GameObject("PlayModeStaticsTests item"));

        yield return new ExitPlayMode();

        // Between sessions: without a domain reload all of it is still there.
        Check((bool)GetStatic(k_singleton, "m_AppIsQuitting"), "between sessions: Singleton<T> is still flagged as quitting");
        Check(m_variable.Value, "between sessions: the variable keeps the previous session's value");
        Check(ListenerCount(m_restartEvent) == 2, "between sessions: the event keeps the previous session's listeners");
        Check(m_set.Items.Count == 1, "between sessions: the runtime set keeps the previous session's item");

        // Session two: PlayModeStatics ran at SubsystemRegistration.
        yield return new EnterPlayMode(expectDomainReload: false);

        Check(!(bool)GetStatic(k_singleton, "m_ShuttingDown"), "session 2: Singleton<T> is not shutting down");
        Check(!(bool)GetStatic(k_singleton, "m_AppIsQuitting"), "session 2: Singleton<T> is not quitting");
        Check(GetStatic(k_singleton, "m_Instance") == null, "session 2: Singleton<T> dropped the previous session's instance");
        Check(PlayModeStaticsTestSingleton.Instance != null, "session 2: Singleton<T>.Instance is created again");
        Check(GetStatic(k_softSingleton, "m_Instance") == null, "session 2: SoftSingleton<T> dropped the previous session's instance");
        Check(!m_variable.Value, "session 2: the variable is back at its default value");
        Check(ListenerCount(m_restartEvent) == 1, "session 2: only the variable's restart listener is registered");
        Check(RestartListenerCount(m_variable) == 1, "session 2: the variable holds one restart listener reference");
        Check(m_set.Items.Count == 0, "session 2: the runtime set is empty");

        yield return new ExitPlayMode();

        Assert.IsEmpty(m_failures, string.Join("\n", m_failures));
    }

    void Check(bool condition, string expectation)
    {
        if (!condition)
            m_failures.Add(expectation);
    }

    static object GetStatic(Type type, string field)
        => type.GetField(field, k_staticFlags).GetValue(null);

    static T GetInstanceField<T>(Type type, object instance, string field)
        => (T)type.GetField(field, k_instanceFlags).GetValue(instance);

    static int ListenerCount(GameEvent gameEvent)
        => GetInstanceField<ICollection>(typeof(GameEvent), gameEvent, "m_eventListenerReferences").Count;

    static int RestartListenerCount(BoolVariable variable)
        => GetInstanceField<ICollection>(typeof(BaseVariable<bool>), variable, "m_restartEventListenerReferences").Count;
}

public class PlayModeStaticsTestSingleton : Singleton<PlayModeStaticsTestSingleton> { }

public class PlayModeStaticsTestSoftSingleton : SoftSingleton<PlayModeStaticsTestSoftSingleton> { }

public class PlayModeStaticsTestRuntimeSet : RuntimeSet<GameObject> { }
