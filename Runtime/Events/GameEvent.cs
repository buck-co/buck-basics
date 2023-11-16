using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Game Event")]
    public class GameEvent : BaseScriptableObject
    {

        
        [Tooltip("Will log all value changes and event calls made by this variable if true and running in Editor or a Development Build.")]
        [SerializeField] protected bool m_debugChanges;


        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<GameEventListener> eventListeners = 
            new List<GameEventListener>();

        public void Raise()
        { 
            #if UNITY_EDITOR || DEVELOPMENTBUILD
            if (m_debugChanges)
            {
                //Log a string containing all event listeners
                System.Text.StringBuilder sB = new System.Text.StringBuilder();
                sB.Append(name + " GameEvent was raised to listeners on the following GameObjects:\n");
                for (int i = eventListeners.Count -1; i >= 0; i--)
                {
                    sB.Append(i + ":" + eventListeners[i].gameObject.name +"\n");
                }
                Debug.Log(sB.ToString());
            }
            #endif


            for(int i = eventListeners.Count -1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(GameEventListener listener)
        {   
            
           
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);

                #if UNITY_EDITOR || DEVELOPMENTBUILD
                if (m_debugChanges)
                {
                    Debug.Log(name + " GameEvent received a new listener attached to this GameObject:" + listener.gameObject.name, listener.gameObject);
                }
                #endif
            }
        }

        public void UnregisterListener(GameEventListener listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);

                #if UNITY_EDITOR || DEVELOPMENTBUILD
                if (m_debugChanges)
                {
                    Debug.Log(name + " GameEvent unregistered a listener attached to this GameObject:" + listener.gameObject.name, listener.gameObject);
                }
                #endif
            }
        }

    }
}