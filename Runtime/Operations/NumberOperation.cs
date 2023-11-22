using System;
using UnityEngine;

namespace Buck
{

    [Serializable]
    public class NumberOperation:BaseOperation
    {
        
        public enum Operations{
            SetTo = 0,//nA=nB
            AdditionAssignment = 1,//nA+=nB
            SubtractionAssignment = 2,//nA-=nB
            MultiplicationAssignment = 3,//nA*=nB
            DivisionAssignment = 4,//nA/=nB
            PowAssignment = 5,//nA^=nB
            Addition = 6,//nA=nB+nC
            Subtraction = 7,//nA=nB-nC
            Multiplication = 8,//nA=nB*nC
            Division = 9,//nA=nB/nC
            Pow = 10,//nA=nB^nC
            };
        
        enum RoundingType{
            RoundToInt=0, 
            FloorToInt=1, 
            CeilToInt=2
            };

        [Tooltip("The NumberVariable that this operation acts on. Supports IntVariables, FloatVariables, or DoubleVariables")]
        [SerializeField] NumberVariable m_numberA;

        [Tooltip("The type of operation to execute. Most, but not all common assignments and math operations are included.")]
        [SerializeField] Operations m_operation;

        [Tooltip("First NumberReference used in the operation. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberB;

        [Tooltip("Second NumberReference possibly used in the operation. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberC;
        
        
        [Tooltip("These rounding operations are only available if NumberA is an IntVariable. They match Unity.Mathf operations of the same name. They are applied as the final step after the operation is calculated as floats.")]
        [SerializeField] RoundingType m_rounding = RoundingType.RoundToInt;

        [Tooltip("If true, when Execute() is called NumberA's event will be raised.")]
        [SerializeField] BoolReference m_raiseEvent;

        [SerializeField, HideInInspector]bool m_serialized = false;
        public override bool Serialized {get=> m_serialized; set{m_serialized = value;}}

        public override void Execute()
        {
            switch (m_numberA.TypeCode)
            {
                case System.TypeCode.Int32:
                    IntVariable intA = (IntVariable)(m_numberA);
                    intA.Value = GetIntResult();

                    break;

                
                case System.TypeCode.Single:
                    FloatVariable floatA = (FloatVariable)(m_numberA);
                    floatA.Value = GetFloatResult();

                    break;

                
                case System.TypeCode.Double:
                    DoubleVariable doubleA = (DoubleVariable)(m_numberA);
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
            switch(m_operation)
            {
                default:
                case Operations.SetTo://nA=nB
                    return m_numberB;

                case Operations.AdditionAssignment://nA+=nB
                    return m_numberA.ValueFloat + m_numberB.ValueFloat;

                case Operations.SubtractionAssignment://nA-=NB
                    return m_numberA.ValueFloat - m_numberB.ValueFloat;

                case Operations.MultiplicationAssignment://nA*=nB
                    return m_numberA.ValueFloat * m_numberB.ValueFloat;

                case Operations.DivisionAssignment://nA/=nB
                    if (m_numberB.ValueFloat == 0f)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberA.ValueFloat / m_numberB.ValueFloat;

                case Operations.PowAssignment://nA^=nB
                    return Mathf.Pow(m_numberA.ValueFloat, m_numberB.ValueFloat);
                    
                case Operations.Addition://nA=nB+nC
                    return m_numberB.ValueFloat + m_numberC.ValueFloat;
                    
                case Operations.Subtraction://nA=nB-nC
                    return m_numberB.ValueFloat - m_numberC.ValueFloat;

                case Operations.Multiplication://nA=nB*nC
                    return m_numberB.ValueFloat * m_numberC.ValueFloat;

                case Operations.Division://nA=nB/nC

                    if (m_numberC.ValueFloat == 0f)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberB.ValueFloat / m_numberC.ValueFloat;

                case Operations.Pow://nA=nB^nC
                    return Mathf.Pow(m_numberB.ValueFloat, m_numberC.ValueFloat);
                    
            }
        }

        public double GetDoubleResult()
        {
            switch(m_operation)
            {
                default:
                case Operations.SetTo://nA=nB
                    return m_numberB;

                case Operations.AdditionAssignment://nA+=nB
                    return m_numberA.ValueDouble + m_numberB.ValueDouble;

                case Operations.SubtractionAssignment://nA-=NB
                    return m_numberA.ValueDouble - m_numberB.ValueDouble;

                case Operations.MultiplicationAssignment://nA*=nB
                    return m_numberA.ValueDouble * m_numberB.ValueDouble;

                case Operations.DivisionAssignment://nA/=nB
                    if (m_numberB.ValueDouble == 0d)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberA.ValueDouble / m_numberB.ValueDouble;

                case Operations.PowAssignment://nA^=nB
                    return (double)(Mathf.Pow(m_numberA.ValueFloat, m_numberB.ValueFloat));
                    
                case Operations.Addition://nA=nB+nC
                    return m_numberB.ValueDouble + m_numberC.ValueDouble;
                    
                case Operations.Subtraction://nA=nB-nC
                    return m_numberB.ValueDouble - m_numberC.ValueDouble;

                case Operations.Multiplication://nA=nB*nC
                    return m_numberB.ValueDouble * m_numberC.ValueDouble;

                case Operations.Division://nA=nB/nC
                    if (m_numberC.ValueDouble == 0d)
                    {
                        Debug.LogWarning("NumberOperation attempted divide by zero. Using Mathf.Infinity as the result.");
                        return Mathf.Infinity;
                    }

                    return m_numberB.ValueDouble / m_numberC.ValueDouble;

                case Operations.Pow://nA=nB^nC
                    return (double)(Mathf.Pow(m_numberB.ValueFloat, m_numberC.ValueFloat));
                    
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