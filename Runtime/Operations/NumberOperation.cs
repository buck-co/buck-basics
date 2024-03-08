using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class NumberOperation:BaseOperation
    {
        public enum Operations
        {
            SetTo = 0,//=
            AdditionAssignment = 1,//+=
            SubtractionAssignment = 2,//-=
            MultiplicationAssignment = 3,//*=
            DivisionAssignment = 4,// /=
            PowAssignment = 5,//^=
        };

        public enum RightHandArithmetic
        {
            None = 0,
            Addition = 1,//+
            Subtraction = 2,//-
            Multiplication = 3,//*
            Division = 4,// /
            Pow = 5,//^
        }
        
        enum RoundingType
        {
            RoundToInt = 0, 
            FloorToInt = 1, 
            CeilToInt = 2
        };

        [Tooltip("The NumberVariable that this operation acts on. Supports IntVariables, FloatVariables, or DoubleVariables")]
        [SerializeField] NumberVariable m_numberA;

        [Tooltip("The type of assignment operation to execute.")]
        [SerializeField] Operations m_operation;

        [Tooltip("An additional modifier of the right side of the assignemtn using common arithmetic. Adds a third NumberReference variable if !None.")]
        [SerializeField] RightHandArithmetic m_rightHandArithmetic;

        [Tooltip("First NumberReference used in the operation. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberB;

        [Tooltip("Second NumberReference possibly used in the operation. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberC;
        
        [Tooltip("These rounding operations are only available if NumberA is an IntVariable. They match Unity.Mathf operations of the same name. They are applied as the final step after the operation is calculated as floats.")]
        [SerializeField] RoundingType m_rounding = RoundingType.RoundToInt;

        [Tooltip("If true, when Execute() is called NumberA's event will be raised.")]
        [SerializeField] BoolReference m_raiseEvent;

        [SerializeField, HideInInspector]bool m_serialized = false;

        public override bool Serialized
        {
            get => m_serialized;
            set => m_serialized = value;
        }

        public override void Execute()
        {
            switch (m_numberA.TypeCode)
            {
                case TypeCode.Int32:
                    IntVariable intA = (IntVariable)m_numberA;
                    intA.Value = GetIntResult();

                    break;

                
                case TypeCode.Single:
                    FloatVariable floatA = (FloatVariable)m_numberA;
                    floatA.Value = GetFloatResult();

                    break;

                
                case TypeCode.Double:
                    DoubleVariable doubleA = (DoubleVariable)m_numberA;
                    doubleA.Value = GetDoubleResult();
                    break;
            }

            if (m_raiseEvent)
                m_numberA.Raise();
        }
        
        public int GetIntResult()
        {
            float floatResult = GetFloatResult();

            switch (m_rounding)
            {
                default:
                case RoundingType.RoundToInt:
                    return Mathf.RoundToInt(floatResult);

                case RoundingType.FloorToInt:
                    return Mathf.FloorToInt(floatResult);

                case RoundingType.CeilToInt:
                    return Mathf.CeilToInt(floatResult);
            }
        }
        
        public float GetFloatResult()
        {
            switch (m_operation)
            {
                default:
                case Operations.SetTo://nA=nB
                    return GetRightHandFloat();

                case Operations.AdditionAssignment://nA+=nB
                    return m_numberA.ValueFloat + GetRightHandFloat();

                case Operations.SubtractionAssignment://nA-=NB
                    return m_numberA.ValueFloat - GetRightHandFloat();

                case Operations.MultiplicationAssignment://nA*=nB
                    return m_numberA.ValueFloat * GetRightHandFloat();

                case Operations.DivisionAssignment://nA/=nB
                    if (GetRightHandFloat() == 0f)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberA.ValueFloat / GetRightHandFloat();

                case Operations.PowAssignment://nA^=nB
                    return Mathf.Pow(m_numberA.ValueFloat, GetRightHandFloat());         
            }
        }

        public float GetRightHandFloat()
        {
            switch (m_rightHandArithmetic)
            {
                default:
                case RightHandArithmetic.None:
                    return m_numberB.ValueFloat;

                case RightHandArithmetic.Addition:
                    return m_numberB.ValueFloat + m_numberC.ValueFloat;
                    
                case RightHandArithmetic.Subtraction:
                    return m_numberB.ValueFloat - m_numberC.ValueFloat;
                    
                case RightHandArithmetic.Multiplication:
                    return m_numberB.ValueFloat * m_numberC.ValueFloat;
                    
                case RightHandArithmetic.Division:
                    if (m_numberC.ValueFloat == 0f)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }
                    return m_numberB.ValueFloat / m_numberC.ValueFloat;

                case RightHandArithmetic.Pow:
                    return Mathf.Pow(m_numberB.ValueFloat, m_numberC.ValueFloat);
            }
        }

        public double GetDoubleResult()
        {
            switch (m_operation)
            {
                default:
                case Operations.SetTo://nA=nB
                    return GetRightHandDouble();

                case Operations.AdditionAssignment://nA+=nB
                    return m_numberA.ValueDouble + GetRightHandDouble();

                case Operations.SubtractionAssignment://nA-=NB
                    return m_numberA.ValueDouble - GetRightHandDouble();

                case Operations.MultiplicationAssignment://nA*=nB
                    return m_numberA.ValueDouble * GetRightHandDouble();

                case Operations.DivisionAssignment://nA/=nB
                    if (GetRightHandDouble() == 0d)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberA.ValueDouble / GetRightHandDouble();

                case Operations.PowAssignment://nA^=nB
                    return (double)(Mathf.Pow(m_numberA.ValueFloat, (float)(GetRightHandDouble()))); // Mathf.Pow doesn't support doubles so cast to float      
            }
        }

        public double GetRightHandDouble()
        {
            switch (m_rightHandArithmetic)
            {
                default:
                case RightHandArithmetic.None:
                    return m_numberB.ValueDouble;

                case RightHandArithmetic.Addition:
                    return m_numberB.ValueDouble + m_numberC.ValueDouble;
                    
                case RightHandArithmetic.Subtraction:
                    return m_numberB.ValueDouble - m_numberC.ValueDouble;
                    
                case RightHandArithmetic.Division:
                    if (m_numberC.ValueDouble == 0d)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }
                    return m_numberB.ValueDouble / m_numberC.ValueDouble;

                case RightHandArithmetic.Pow:
                    return Mathf.Pow(m_numberB.ValueFloat, m_numberC.ValueFloat);//Mathf.Pow doesn't support doubles, so cast to floats
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
    }
}
