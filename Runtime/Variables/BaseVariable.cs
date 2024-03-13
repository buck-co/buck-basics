using UnityEngine;

namespace Buck
{
    public abstract class BaseVariable<T> : GameEvent
    {
        [SerializeField] protected T DefaultValue;
        
        protected T m_currentValue;

        public virtual T Value
        {
            get => m_currentValue;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public virtual string ValueAsString
            => m_currentValue.ToString();

        // Should we remove this? It's functionally equivalent to the Value setter but without the logging.
        public void SetValue(T value)
            => Value = value;
        
        protected virtual void OnEnable()
            => m_currentValue = DefaultValue;

        public void LogValueChange()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " set to: " + ValueAsString);
#endif
        }

#if UNITY_EDITOR
        public void LogValue()
            => Debug.Log("Value of " + name + " is: " + ValueAsString);
#endif
    }
}
