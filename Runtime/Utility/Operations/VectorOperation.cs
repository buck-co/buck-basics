using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class VectorOperation:BaseOperation
    {
        public enum Operations{
            SetTo = 0,//vA=vB
            AdditionAssignment = 1,//vA+=vB
            SubtractionAssignment = 2,//vA-=vB
            Addition = 3,//vA=vB+vC
            Subtraction = 4,//vA=vB-vC
            MultiplyByScalar = 5,//vA*=nS
            DivideByScalar = 6//vA/=nS
            };

        [SerializeField] VectorVariable m_vectorA;

        [SerializeField] Operations m_operation;

        [SerializeField] VectorReference m_vectorB;
        [SerializeField] VectorReference m_vectorC;
        [SerializeField] NumberReference m_numberScalar;//Used for scalar operations
        

        [SerializeField] bool m_raiseEvent = true;


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
                            vector2Variable.Value = (Vector2)(GetVector4Result());
                            break;
                        default:
                        case 3:
                            Vector3Variable vector3Variable = (Vector3Variable)(m_vectorA);
                            vector3Variable.Value = (Vector3)(GetVector4Result());
                            break;
                        case 4:
                            Vector4Variable vector4Variable = (Vector4Variable)(m_vectorA);
                            vector4Variable.Value = GetVector4Result();
                            break;

                    }
                    
                    break;

                case true:
                    switch(m_vectorA.VectorLength)
                    {
                        case 2:
                            Vector2IntVariable vector2IntVariable = (Vector2IntVariable)(m_vectorA);
                            vector2IntVariable.Value = (Vector2Int)(GetVector3IntResult());
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
            switch(m_operation)
            {
                default:
                case Operations.SetTo://vA=vB
                    return m_vectorB;

                case Operations.AdditionAssignment://vA+=vB
                    return m_vectorA.ValueVector4 + m_vectorB.ValueVector4;

                case Operations.SubtractionAssignment://vA-=vB
                    return m_vectorA.ValueVector4 - m_vectorB.ValueVector4;
                    
                case Operations.Addition://vA=vB+vC
                    return m_vectorB.ValueVector4 + m_vectorC.ValueVector4;
                    
                case Operations.Subtraction://vA=vB-vC
                    return m_vectorB.ValueVector4 - m_vectorC.ValueVector4;

                case Operations.MultiplyByScalar://vA=vB*nS
                    return m_vectorA.ValueVector4 * m_numberScalar.ValueFloat;

                case Operations.DivideByScalar://nA=nB/nS
                    return m_vectorA.ValueVector4 / m_numberScalar.ValueFloat;

            }
        }

        /// <summary>
        /// Returns the result of what VectorA would be set to if the operation happened as a Vector3Int (ints). Does not actually execute the result.
        /// Optionally, use GetVector4Result() if you want to get the result as a floating point Vector4();
        /// Useful for writing code that will query what will happen if the event executes.
        /// </summary>   
        public Vector3Int GetVector3IntResult()
        {
            switch(m_operation)
            {
                default:
                case Operations.SetTo://vA=vB
                    return m_vectorB.ValueVector3Int;

                case Operations.AdditionAssignment://vA+=vB
                    return m_vectorA.ValueVector3Int + m_vectorB.ValueVector3Int;

                case Operations.SubtractionAssignment://vA-=vB
                    return m_vectorA.ValueVector3Int - m_vectorB.ValueVector3Int;
                    
                case Operations.Addition://vA=vB+vC
                    return m_vectorB.ValueVector3Int + m_vectorC.ValueVector3Int;
                    
                case Operations.Subtraction://vA=vB-vC
                    return m_vectorB.ValueVector3Int - m_vectorC.ValueVector3Int;

                case Operations.MultiplyByScalar://vA=vB*nS
                    return m_vectorA.ValueVector3Int * m_numberScalar.ValueInt;

                case Operations.DivideByScalar://nA=nB/nS
                    return m_vectorA.ValueVector3Int / m_numberScalar.ValueInt;

            }
        }

    }
}