using UnityEngine;

namespace Buck
{
    public class BaseVariable : GameEvent
    {
        public virtual string ValueAsString => "ValueAsString unimplemented for this type of BaseVariable.";

        public void LogValueChange()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " set to: " + ValueAsString);
#endif
        }

#if UNITY_EDITOR
        public void LogValue()
        {
            Debug.Log("Value of " + name + " is: " + ValueAsString);
        }
#endif
    }
}
