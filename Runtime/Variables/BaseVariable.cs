using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Buck
{
    public abstract class BaseVariable<T> : GameEvent, IFormattable
    {
        [FormerlySerializedAs("DefaultValue")] [SerializeField] protected T m_defaultValue;
        [SerializeField] protected bool m_resetOnRestart = false;
        [SerializeField] protected List<GameEvent> m_restartEvents = new List<GameEvent>();

        protected T m_currentValue;
        protected List<GameEventListenerReference> m_restartEventListenerReferences = new List<GameEventListenerReference>();
        
        public virtual T Value
        {
            get => m_currentValue;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ToString()
            => Value.ToString();

        public virtual string ToString(string format, IFormatProvider formatProvider)
            => Value.ToString();

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
        
        void ResetValueToDefault()
            => Value = m_defaultValue;
        
        void OnRestartEventRaised()
        {
            if (m_resetOnRestart)
            {
                ResetValueToDefault();
                Raise();
            }
        }

        protected void LogValueChange()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (m_debugChanges)
                Debug.Log("Value of " + name + " set to: " + ToString());
#endif
        }

#if UNITY_EDITOR
        public void LogValue()
            => Debug.Log("Value of " + name + " is: " + ToString());
#endif
        
        
    }
}
