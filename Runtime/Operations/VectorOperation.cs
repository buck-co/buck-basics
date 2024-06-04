using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class VectorOperation : BaseOperation
    {
        public enum Operations
        {
            SetTo = 0,//vA=vB
            AdditionAssignment = 1,//vA+=vB
            SubtractionAssignment = 2,//vA-=vB
        };

        public enum RightHandArithmetic
        {
            None = 0,
            Addition = 1,//+
            Subtraction = 2,//-
            ScalarMultiplication = 3,//*
            ScalarDivision = 4,// /
        }

        [Tooltip("The VectorVariable that this operation acts on. Supports Vector2Variables, Vector3Variables, Vector4Variables, Vector2IntVariables, or Vector3IntVariables.")]
        [SerializeField] VectorVariable m_vectorA;

        [Tooltip("The type of assignment operation to execute. Scalar multiplication + division is not supported here, but instead done using the Right Hand Arithmetic setting.")]
        [SerializeField] Operations m_operation;

        [Tooltip("An additional modifier of the right side of the assignment using common arithmetic. Adds a third variable if !None.")]
        [SerializeField] RightHandArithmetic m_rightHandArithmetic;

        [Tooltip("First VectorReference used in the operation. Supports a constant Vector4, Vector2Variables, Vector3Variables, Vector4Variables, Vector2IntVariables, or Vector3IntVariables.")]
        [SerializeField] VectorReference m_vectorB;

        [Tooltip("Second VectorReference possibly used in the operation. Supports a constant Vector4, Vector2Variables, Vector3Variables, Vector4Variables, Vector2IntVariables, or Vector3IntVariables.")]
        [SerializeField] VectorReference m_vectorC;
        
        [Tooltip("A NumberReference used in a few of the operations as a scalar. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberScalar; // Used for scalar operations
        

        [Tooltip("If true, when Execute() is called VectorA's event will be raised.")]
        [SerializeField] BoolReference m_raiseEvent;
        [SerializeField, HideInInspector]bool m_serialized = false;

        public override bool Serialized
        {
            get => m_serialized;
            set => m_serialized = value;
        }
            
        public override void Execute()
        {
            switch (m_vectorA.IsAVectorInt)
            {
                default:
                case false:
                    switch(m_vectorA.VectorLength)
                    {
                        case 2:
                            Vector2Variable vector2Variable = (Vector2Variable)(m_vectorA);
                            vector2Variable.Value = (Vector2)GetVector4Result();
                            break;
                        default:
                        case 3:
                            Vector3Variable vector3Variable = (Vector3Variable)(m_vectorA);
                            vector3Variable.Value = (Vector3)GetVector4Result();
                            break;
                        case 4:
                            Vector4Variable vector4Variable = (Vector4Variable)(m_vectorA);
                            vector4Variable.Value = GetVector4Result();
                            break;
                    }
                    break;

                case true:
                    switch (m_vectorA.VectorLength)
                    {
                        case 2:
                            Vector2IntVariable vector2IntVariable = (Vector2IntVariable)(m_vectorA);
                            vector2IntVariable.Value = (Vector2Int)GetVector3IntResult();
                            break;
                        default:
                        case 3:
                            Vector3IntVariable vector3IntVariable = (Vector3IntVariable)(m_vectorA);
                            vector3IntVariable.Value = GetVector3IntResult();
                            break;
                    }
                    break;
            }

            if (m_raiseEvent)
                m_vectorA.Raise();
        }

        /// <summary>
        /// Returns the result of what VectorA would be set to if the operation happened as a Vector4 (floats). Does not actually execute the result.
        /// Optionally, use GetVectorIntResult if you want to get the result as a int-based Vector3Int();
        /// Useful for writing code that will query what will happen if the event executes.
        /// </summary>       
        public Vector4 GetVector4Result()
        {
            switch (m_operation)
            {
                default:
                case Operations.SetTo://vA=vB
                    return GetRightHandVector4();

                case Operations.AdditionAssignment://vA+=vB
                    return m_vectorA.ValueVector4 + GetRightHandVector4();

                case Operations.SubtractionAssignment://vA-=vB
                    return m_vectorA.ValueVector4 - GetRightHandVector4();
            }
        }

        public Vector4 GetRightHandVector4()
        {
            switch (m_rightHandArithmetic)
            {
                default:
                case RightHandArithmetic.None:
                    return m_vectorB.ValueVector4;

                case RightHandArithmetic.Addition:
                    return m_vectorB.ValueVector4 + m_vectorC.ValueVector4;
                
                case RightHandArithmetic.Subtraction:
                    return m_vectorB.ValueVector4 - m_vectorC.ValueVector4;
                
                case RightHandArithmetic.ScalarMultiplication:
                    return m_vectorB.ValueVector4 * m_numberScalar.ValueFloat;
                
                case RightHandArithmetic.ScalarDivision:
                    return m_vectorB.ValueVector4 / m_numberScalar.ValueFloat;
            }
        }

        /// <summary>
        /// Returns the result of what VectorA would be set to if the operation happened as a Vector3Int (ints). Does not actually execute the result.
        /// Optionally, use GetVector4Result() if you want to get the result as a floating point Vector4();
        /// Useful for writing code that will query what will happen if the event executes.
        /// </summary>         
        public Vector3Int GetVector3IntResult()
        {
            switch (m_operation)
            {
                default:
                case Operations.SetTo://vA=vB
                    return GetRightHandVector3Int();

                case Operations.AdditionAssignment://vA+=vB
                    return m_vectorA.ValueVector3Int + GetRightHandVector3Int();

                case Operations.SubtractionAssignment://vA-=vB
                    return m_vectorA.ValueVector3Int - GetRightHandVector3Int();

            }
        }

        public Vector3Int GetRightHandVector3Int()
        {
            switch (m_rightHandArithmetic)
            {
                default:
                case RightHandArithmetic.None:
                    return m_vectorB.ValueVector3Int;

                case RightHandArithmetic.Addition:
                    return m_vectorB.ValueVector3Int + m_vectorC.ValueVector3Int;
                
                case RightHandArithmetic.Subtraction:
                    return m_vectorB.ValueVector3Int - m_vectorC.ValueVector3Int;
                
                case RightHandArithmetic.ScalarMultiplication:
                    return m_vectorB.ValueVector3Int * m_numberScalar.ValueInt;
                
                case RightHandArithmetic.ScalarDivision:
                    return m_vectorB.ValueVector3Int / m_numberScalar.ValueInt;
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
        public void SetValues(VectorVariable vectorA, VectorReference vectorB, VectorReference vectorC, NumberReference scalar, Operations operation, RightHandArithmetic rightHandArithmetic)
        {
            m_vectorA = vectorA;
            m_vectorB = vectorB;
            m_vectorC = vectorC;
            m_operation = operation;
            m_numberScalar = scalar;
            m_rightHandArithmetic = rightHandArithmetic;
            m_raiseEvent = new BoolReference(false);
        }
        
        public VectorVariable VectorA => m_vectorA;
        public VectorReference VectorB => m_vectorB;
        public VectorReference VectorC => m_vectorC;
#endif
    }
}
