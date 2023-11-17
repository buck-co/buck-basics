using UnityEngine;

namespace Buck
{
    public class BaseVariable : GameEvent
    {
        //TODO: Check if this could be abstract
        protected virtual string DebugValue => "BaseVariable has no value. This should be overridden";
        protected void LogValueChange()
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " set to: " + DebugValue);
            #endif
        }
    }
}