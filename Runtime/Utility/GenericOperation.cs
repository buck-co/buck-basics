using UnityEngine;
using UnityEngine.Events;

namespace Buck
{
    /// <summary>
    /// This is a useful MonoBehaviour component that can leverage BUCK Conditions and Operations in a generic way.
    /// Call the public ExecuteIfConditionsPassed() method to execute all of the Operations defined, raise all GameEvents 
    /// added, and invoke the UnityEvent defined. Coupling this component with a GameEventListener which triggers
    /// ExecuteIfConditionsPassed() when it's GameEvent is raised works well.
    /// ExecuteIfConditionsPassed() is virtual if you want to extend this class. It returns true if the conditions were passed
    /// and the operations were executed or false if the conditions were not passed.
    /// </summary>
    public class GenericOperation : MonoBehaviour
    {
        [Tooltip("Conditions that must be met for execution to happen when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] Condition[] m_conditions;

        [Tooltip("Optional BoolOperations that execute if conditions are passed when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] BoolOperation[] m_boolOperations;
        
        [Tooltip("Optional NumberOperations that execute if conditions are passed when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] NumberOperation[] m_numberOperations;
        
        [Tooltip("Optional VectorOperations that execute if conditions are passed when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] VectorOperation[] m_vectorOperations;
        
        [Tooltip("Optional BUCK GameEvents that raise if conditions are passed when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] GameEvent[] m_gameEvents;

        [Tooltip("Optional UnityEvent that invokes if conditions are passed when ExecuteIfConditionsPassed() is called.")]
        [SerializeField] UnityEvent m_unityEvent;

        /// <summary>
        /// If the defined Conditions are passed each defined Operation will execute, GameEvents will raise, and the UnityEvent will invoke.
        /// </summary>
        public virtual bool ExecuteIfConditionsPassed()
        {
            if (m_conditions.PassConditions())
            {
                m_boolOperations.Execute();
                m_numberOperations.Execute();
                m_vectorOperations.Execute();

                foreach(GameEvent gE in m_gameEvents)
                    gE.Raise();
    
                m_unityEvent.Invoke();
                return true;
            }

            return false;
        }
    }
}
