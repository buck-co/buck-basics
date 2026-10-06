// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// Restores the package's static and asset-held runtime state at the start of every Play
    /// session, so BUCK Basics behaves the same whether or not the editor reloads the scripting
    /// domain on Play (Edit > Project Settings > Editor > Enter Play Mode Settings). Unity's
    /// CoreCLR runtime removes domain reload altogether, so this is also what keeps the package
    /// working there.
    ///
    /// Two kinds of state need it:
    /// - Statics on generic types (Singleton&lt;T&gt;, SoftSingleton&lt;T&gt;). Unity does not invoke
    ///   RuntimeInitializeOnLoadMethod inside generic types, so each closed type registers a reset
    ///   action from its static constructor and this class runs them all.
    /// - Runtime fields on ScriptableObject assets (GameEvent listeners, BaseVariable values,
    ///   RuntimeSet items). Those were reset by OnEnable after each domain reload; without a reload
    ///   an asset that is already loaded gets no OnEnable, so the values of the previous session
    ///   would carry into the next one.
    ///
    /// The reset runs at SubsystemRegistration, the earliest stage, before any scene object's
    /// Awake, so nothing registered by the new session itself is ever touched. Everything here is
    /// a no-op in a player and in an editor that still reloads the domain: there is nothing stale
    /// to reset at that point.
    /// </summary>
    public static class PlayModeStatics
    {
        static readonly List<Action> m_resets = new List<Action>();

        /// <summary>
        /// Registers an action that puts a type's statics back to their initial values. Called
        /// once per closed generic type from its static constructor; the list itself is never
        /// cleared, since the registration has to outlive every Play session.
        /// </summary>
        public static void Register(Action reset)
        {
            if (reset != null && !m_resets.Contains(reset))
                m_resets.Add(reset);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetForPlaySession()
        {
            for (int i = 0; i < m_resets.Count; i++)
                m_resets[i]();

            // Every loaded GameEvent (variables and runtime sets derive from it). Listener lists
            // first, so a variable re-registering its restart listeners below is not cleared again.
            var events = Resources.FindObjectsOfTypeAll<GameEvent>();
            for (int i = 0; i < events.Length; i++)
                events[i].ClearRuntimeListeners();
            for (int i = 0; i < events.Length; i++)
                events[i].OnPlaySessionStarted();
        }
    }
}
