// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.Events;

namespace Buck
{
    public class GameEventListenerReference
    {
        public GameEvent Event  { get; set; }
        public delegate void OnEventRaised();
        public OnEventRaised OnEventRaisedDelegate;
        public Object EventListener;
    }
    
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;
        
        GameEventListenerReference m_gameEventListenerReference;

        void OnEnable()
        {
            m_gameEventListenerReference = new GameEventListenerReference
            {
                EventListener = gameObject,
                Event = Event,
                OnEventRaisedDelegate = OnEventRaised
            };
            
            if (m_gameEventListenerReference.Event != null)
                m_gameEventListenerReference.Event.RegisterListener(m_gameEventListenerReference);
        }

        void OnDisable()
        {
            if (m_gameEventListenerReference.Event != null)
                m_gameEventListenerReference.Event.UnregisterListener(m_gameEventListenerReference);
        }

        public void OnEventRaised()
        {
            if (Response != null)
                Response.Invoke();
        }
    }
}
