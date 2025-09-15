// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using UnityEngine;
#if BUCK_BASICS_ENABLE_LOCALIZATION
using UnityEngine.Localization;
#endif

namespace Buck
{
    public abstract class BaseVariable : GameEvent
    {
        public abstract Type Type { get; }
        public abstract string LabelText { get; }
    }
    
    public abstract class BaseVariable<T> : BaseVariable, IFormattable
    {
        [SerializeField, Tooltip("The initial value of this variable when the application starts.")] protected T m_defaultValue;

        [SerializeField, Tooltip("The label text to use when displaying this variable in user-facing UI.")] string m_labelText;
#if BUCK_BASICS_ENABLE_LOCALIZATION
        [SerializeField, Tooltip("When enabled, the LabelText property will use the \"Localized Label Text\" instead of the \"Label Text\" string.")]
        bool m_localizeLabelText = false;
        [SerializeField] LocalizedString m_localizedLabelText;
#endif

        /// <summary>
        /// The label text to use when displaying this variable in user-facing UI. Can be localized if localization is enabled.
        /// </summary>
        public override string LabelText
        {
            get
            {
#if BUCK_BASICS_ENABLE_LOCALIZATION
                // If localization is enabled and a localized string is set, use it
                if (m_localizeLabelText && m_localizedLabelText != null)
                    return m_localizedLabelText.GetLocalizedString();
#endif
                return m_labelText;
            }
        }
        
        [SerializeField, Tooltip("When enabled, if any of the \"Restart Events\" are raised, this variable will be reset to its Default Value. " +
                                 "This is useful for resetting variables when a new game or level starts.")]
        protected bool m_resetOnRestart = false;
        
        [SerializeField] protected List<GameEvent> m_restartEvents = new List<GameEvent>();

        protected T m_currentValue;
        protected List<GameEventListenerReference> m_restartEventListenerReferences = new List<GameEventListenerReference>();
        
        public override Type Type
            => typeof(T);
        
        public T Value
        {
            get => m_currentValue;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ToString()
            => Value != null ? Value.ToString() : name + ".Value is null.";

        public virtual string ToString(string format, IFormatProvider formatProvider)
            => Value != null ? Value.ToString() : name + ".Value is null.";

        protected virtual void OnEnable()
        {
            ResetValueToDefault();
            m_restartEventListenerReferences.Clear();
            
            foreach (var restartEvent in m_restartEvents)
            {
                var restartEventListenerReference = new GameEventListenerReference
                {
                    EventListener = this,
                    Event = restartEvent,
                    OnEventRaisedDelegate = OnRestartEventRaised
                };
                
                restartEvent.RegisterListener(restartEventListenerReference);
                m_restartEventListenerReferences.Add(restartEventListenerReference);
            }
        }
        
        protected virtual void OnDisable()
        {
            foreach (var restartEventListenerReference in m_restartEventListenerReferences)
                restartEventListenerReference.Event.UnregisterListener(restartEventListenerReference);
            
            m_restartEventListenerReferences.Clear();
        }
        
        public void ResetValueToDefault()
            => Value = m_defaultValue;
        
        void OnRestartEventRaised()
        {
            if (!m_resetOnRestart) return;
            ResetValueToDefault();
            Raise();
        }

        protected void LogValueChange()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (!m_debugChanges) return;
            
            if (Value != null)
                Debug.Log("Value of " + name + " set to " + ToString());
            else
                Debug.Log("Value of " + name + " set to null.");
#endif
        }

#if UNITY_EDITOR
        public void LogValue()
        {
            if (Value != null)
                Debug.Log("Value of " + name + " is " + ToString());
            else
                Debug.Log("Value of " + name + " is null.");
        }
#endif


    }
}
