// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    /// <summary>
    /// An alternate singleton format that allows deleting and respawning new Singletons at runtime. Instead of auto generating whenever referenced like our default Singleton, 
    /// it will become the Instance from it's Awake method or a public call to SetAsSingleton(). This is better for Singletons that are unique to a single scene or other limited
    /// period of the game's overall runtime, since this singleton does not lock out the possibility of a new Singleton ever taking over.
    /// Unlike Singleton<T>, it is possible for a reference to Instance to be null, so you must ensure that the SoftSingleton exists and is set up before using it.
    /// </summary>
    public class SoftSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T m_Instance;

        [SerializeField] bool m_dontDestroyOnLoad = true;

        /// <summary>
        /// Access singleton instance through this property.
        /// </summary>
        public static T Instance
            => m_Instance;


        protected virtual void Awake()
            => SetAsSingleton();

        /// <summary>
        /// Public method for attempting to set the target SoftSingleton to the Instance. Will return true if it succeeds (no Instance already set) or false if it fails (Instance already filled).
        /// SetAsSingleton() is called in SoftSingleton.Awake() as long as Awake isn't overridden.
        /// </summary>
        public bool SetAsSingleton()
        {
            if (m_Instance == null)
            {
                m_Instance = gameObject.GetComponent<T>();

                if (m_dontDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);

                return true;
            }

            return false;
        }

    }
}