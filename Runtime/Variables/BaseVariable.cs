using System;
using UnityEngine;

namespace Buck
{
    public abstract class BaseVariable<T> : GameEvent, IFormattable
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
        
        public override string ToString()
            => m_currentValue.ToString();

        public virtual string ToString(string format, IFormatProvider formatProvider)
            => m_currentValue.ToString();
        
        protected virtual void OnEnable()
            => m_currentValue = DefaultValue;

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
