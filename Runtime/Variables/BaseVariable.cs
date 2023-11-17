using UnityEngine;

namespace Buck
{
    public class BaseVariable : GameEvent
    {
        protected void LogValueChange(string valueAsString)
        {
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " set to: " + valueAsString);
            #endif
        }
    }
}