// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        static bool m_ShuttingDown = false;
        static bool m_AppIsQuitting = false;
        static object m_Lock = new object();
        static T m_Instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed. Returning null.");
                    return null;
                }

                lock (m_Lock)
                {
                    if (m_Instance == null)
                    {
                        // Search for existing instance.
                        m_Instance = (T)FindAnyObjectByType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (m_Instance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            m_Instance = singletonObject.AddComponent<T>();
                            singletonObject.name = typeof(T).ToString() + " (Singleton)";

                            // Make instance persistent.
                            DontDestroyOnLoad(singletonObject);
                        }
                    }

                    return m_Instance;
                }
            }
        }

        void OnApplicationQuit()
        {
            m_AppIsQuitting = true;
            m_ShuttingDown = true;
        }

        void OnDestroy()
        {
            if (ReferenceEquals(m_Instance, this))
            {
                if (m_AppIsQuitting)
                {
                    // If the application is quitting, don't allow recreation.
                    m_ShuttingDown = true;
                }
                else
                {
                    // If the instance is destroyed because of scene reload / swap, allow recreation.
                    m_Instance = null;
                    m_ShuttingDown = false;
                }
            }
            // If this wasn't the active instance (duplicate), don't do anything.
        }
    }
}