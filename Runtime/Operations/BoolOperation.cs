using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class BoolOperation : BaseOperation
    {
        public enum Operations
        {
            SetTo = 0,//bA=bB
            Toggle = 1//bA=!bA
        };
        
        // Constructor
        public BoolOperation(BoolVariable boolA, BoolReference boolB, Operations operation, BoolReference raiseEvent)
        {
            m_boolA = boolA;
            m_boolB = boolB;
            m_operation = operation;
            m_raiseEvent = raiseEvent;
        }
        
        // Parameterless constructor
        public BoolOperation()
        { }
        
        [Tooltip("The BoolVariable that this operation acts on.")]
        [SerializeField] BoolVariable m_boolA;
        public BoolVariable BoolA => m_boolA;

        [Tooltip("The type of operation to execute. SetTo sets BoolA to BoolReference. Toggle will flip the value of BoolA (BoolA = !BoolA)")]
        [SerializeField] Operations m_operation;
        public Operations Operation => m_operation;

        [Tooltip("The value BoolA will be set to when the operation executes.")]
        [SerializeField] BoolReference m_boolB;
        public BoolReference BoolB => m_boolB;

        [Tooltip("If true, when Execute() is called BoolA's event will be raised.")]
        [SerializeField] BoolReference m_raiseEvent = new BoolReference(true);
        public BoolReference RaiseEvent => m_raiseEvent;
        
        [SerializeField, HideInInspector] bool m_serialized = false;

        public override bool Serialized
        {
            get => m_serialized;
            set => m_serialized = value;
        }

        /// <summary>
        /// Applies the operation and sets BoolA. Raises BoolA's event if RaiseEvent is true.
        /// </summary>
        public override void Execute()
        {
            m_boolA.Value = GetResult();

            if (m_raiseEvent)
                m_boolA.Raise();
        }

        /// <summary>
        /// Returns the result of what BoolA would be set to if the operation happened. Does not actually execute the result. 
        /// Useful for writing code that will query what will happen if the event executes.
        /// </summary>
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

        public override void OnBeforeSerialize()
        {
            
        }

        public override void OnAfterDeserialize()
        {
            if (!Serialized)
            {
                m_raiseEvent = new BoolReference(true);
                Serialized = true;
            }
        }
        
#if UNITY_INCLUDE_TESTS
        public void SetValues(BoolVariable boolA, BoolReference boolB, Operations operation)
        {
            m_boolA = boolA;
            m_boolB = boolB;
            m_operation = operation;
        }
#endif
    }
}
