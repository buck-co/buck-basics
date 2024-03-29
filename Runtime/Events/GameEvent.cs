using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Game Event")]
    public class GameEvent : BaseScriptableObject
    {
        [Tooltip("Will log all runtime interactions with this object if true and running in Editor or a Development Build.")]
        [SerializeField] protected bool m_debugChanges;
        
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        readonly List<GameEventListenerReference> m_eventListenerReferences = new();
        
        string GetListenerName(GameEventListenerReference listenerReference)
            => listenerReference.EventListener is GameObject gameObject
            ? gameObject.gameObject.name
            : listenerReference.EventListener.name;
        
        bool ListenerIsGameObject(GameEventListenerReference listenerReference)
            => listenerReference.EventListener is GameObject;

        public void Raise()
        { 
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (m_debugChanges)
            {
                if (m_eventListenerReferences.Count == 0)
                    Debug.Log(name + " GameEvent was raised, but it has 0 listeners subscribed.");
                else
                {
                    //Log a string containing all event listeners
                    System.Text.StringBuilder sB = new System.Text.StringBuilder();
                    sB.Append(name + " GameEvent was raised to listeners on the following GameObjects:\n");
                    for (int i = m_eventListenerReferences.Count - 1; i >= 0; i--)
                        if (m_eventListenerReferences[i].EventListener != null)
                            sB.Append(i + ":" + GetListenerName(m_eventListenerReferences[i]) +"\n");
                    
                    Debug.Log(sB.ToString());
                }
            }
#endif
            for(int i = m_eventListenerReferences.Count -1; i >= 0; i--)
                m_eventListenerReferences[i].OnEventRaisedDelegate();
        }

        public void RegisterListener(GameEventListenerReference listenerReference)
        {   
            if (!m_eventListenerReferences.Contains(listenerReference))
            {
                m_eventListenerReferences.Add(listenerReference);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges && listenerReference.EventListener != null)
                {
                    if (ListenerIsGameObject(listenerReference))
                        Debug.Log(name + " GameEvent received a new listener attached to this GameObject: " +
                                  GetListenerName(listenerReference), (GameObject)listenerReference.EventListener);
                    else
                        Debug.Log(name + " GameEvent received a new listener attached to this object: " +
                                  GetListenerName(listenerReference));
                }
#endif
            }
        }

        public void UnregisterListener(GameEventListenerReference listenerReference)
        {
            if (m_eventListenerReferences.Contains(listenerReference))
            {
                m_eventListenerReferences.Remove(listenerReference);

#if UNITY_EDITOR || DEVELOPMENTBUILD
                if (m_debugChanges && listenerReference.EventListener != null)
                {
                    if (ListenerIsGameObject(listenerReference))
                        Debug.Log(name + " GameEvent unregistered a listener attached to this GameObject: " +
                                  GetListenerName(listenerReference), (GameObject)listenerReference.EventListener);
                    else
                        Debug.Log(name + " GameEvent unregistered a listener attached to this object: " +
                                  GetListenerName(listenerReference));
                }
#endif
            }
        }
    }
}
