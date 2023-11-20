using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class BoolOperation
    {
        
        public enum Operations{
            SetTo = 0,//bA=bB
            Toggle = 1//bA=!bA
            };
            

        [SerializeField] BoolVariable m_boolA;

        [SerializeField] Operations m_operation;
        [SerializeField] BoolReference m_boolB;

        [Tooltip("If true, when Execute() is called m_boolA's event will be raised.")]
        [SerializeField] bool m_raiseEvent = new BoolReference(true);

        /// <summary>
        /// Applies the operation and sets BoolA. Raises BoolA's event if RaiseEvent is true.
        /// </summary>
        public void Execute()
        {
            m_boolA.Value = GetResult();

            if (m_raiseEvent)
                m_boolA.Raise();
        }

        /// <summary>
        /// Returns the result of what BoolA would be set to if the operation happened. Does not actually execute the result. 
        /// Useful for writing code that will query what will happen if the event executes.
        /// </summary>
        /// <returns></returns>
        public bool GetResult()
        {
            switch (m_operation)
            {
                default:
                case Operations.SetTo:
                    return m_boolB.Value;
                
                case Operations.Toggle:
                    return !m_boolA.Value;
            }
        }

    }
}