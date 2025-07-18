// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

namespace Buck
{
    public static class ConditionExtensionMethods
    {        
        /// <summary>
        /// Loops through a ICollection of Conditions and returns true if they all pass, false if any of them fail.
        /// </summary>
        public static bool PassConditions(this ICollection<Condition> conditions)
        {
            if (conditions == null)
                return true;

            foreach(Condition c in conditions)
                if (!c.PassCondition)
                    return false;

            return true;
        }

        /// <summary>
        /// Loops through a array of Conditions and returns true if they all pass, false if any of them fail.
        /// </summary>
        public static bool PassConditions(this Condition[] conditions)
        {
            if (conditions == null)
                return true;
            
            foreach(Condition c in conditions)
                if (!c.PassCondition)
                    return false;

            return true;
        }
    }
    
    /*Usage example (in a Monobehaviour):
        [SerializeField] Condition m_singleCondition; //Define this in the monobehaviour's inspector

        [SerializeField] Condition[] m_multipleConditions;//Define these in the monobehaviour's inspector

        void Check()
        {
            if (m_singleCondition.PassCondition)//Will return true if the single condition is passed
            {
            }

            if (m_multipleConditions.PassConditions())//Will return true if ALL the conditions in the array are passed (false if any are false)
            {
                
            }
        }
    */
    
    /// <summary>
    /// A serializable class that can be used to define basic boolean conditional logic comparing two variables. 
    /// Supports Int, Bool, Float, and Vector3. Uses Buck Basic VariableReferences to support optionally referencing Buck Basics Scriptable Object Variables.
    /// See script for usage examples./// 
    /// </summary>
    [Serializable]
    public class Condition
    {
        public enum BooleanComparisons
        {
            EqualTo,
            NotEqualTo,
            LessThan,
            LessThanOrEqualTo,
            GreaterThan,
            GreaterThanOrEqualTo
        }
        
        public enum VariableType
        {
            Bool = 0, 
            Number = 1, 
            Vector = 2
        };
        
        /// <summary>
        /// Constructor for Booleans
        /// </summary>
        public Condition(BoolReference boolA, BooleanComparisons comparison, BoolReference boolB)
        {
            m_variableType = VariableType.Bool;
            m_boolA = boolA;
            m_comparison = comparison;
            m_boolB = boolB;
        }
        
        /// <summary>
        /// Constructor for Numbers
        /// </summary>
        public Condition(NumberReference numberA, BooleanComparisons comparison, NumberReference numberB)
        {
            m_variableType = VariableType.Number;
            m_numberA = numberA;
            m_comparison = comparison;
            m_numberB = numberB;
        }
        
        /// <summary>
        /// Constructor for Vectors
        /// </summary>
        public Condition(VectorReference vectorA, BooleanComparisons comparison, VectorReference vectorB)
        {
            m_variableType = VariableType.Vector;
            m_vectorA = vectorA;
            m_comparison = comparison;
            m_vectorB = vectorB;
        }
        
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public Condition()
        { }

        [Tooltip("Use the dropdown to pick which type of Variable you wish to compare.")]
        [SerializeField] VariableType m_variableType = VariableType.Bool;
        public VariableType VariableTypeProperty => m_variableType;
        
        [Tooltip("The left side Bool in condition boolean logic.")]
        [SerializeField] BoolReference m_boolA;
        public BoolReference BoolA => m_boolA;
        
        [Tooltip("The left side NumberReference in condition boolean logic. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberA;
        public NumberReference NumberA => m_numberA;
        
        [Tooltip("The left side VectorReference in condition boolean logic. Supports a constant Vector4, Vector2Variables, Vector3Variables, Vector4Variables, Vector2IntVariables, or Vector3IntVariables.")]
        [SerializeField] VectorReference m_vectorA;
        public VectorReference VectorA => m_vectorA;

        [Tooltip("Use the dropdown to pick what kind of boolean comparison you want to use to compare the two reference objects. For size comparisons, Booleans will cast to 0 (false) or 1 (true). Vectors will use each of their magnitudes.")]
        [SerializeField] BooleanComparisons m_comparison = BooleanComparisons.EqualTo;
        public BooleanComparisons Comparison => m_comparison;
        
        [Tooltip("The right side Bool in condition boolean logic.")]
        [SerializeField] BoolReference m_boolB;
        public BoolReference BoolB => m_boolB;

        [Tooltip("The right side NumberReference in condition boolean logic. Supports a constant Float, IntVariables, FloatVariables, or DoubleVariables.")]
        [SerializeField] NumberReference m_numberB;
        public NumberReference NumberB => m_numberB;
        
        [Tooltip("The right side VectorReference in condition boolean logic. Supports a constant Vector4, Vector2Variables, Vector3Variables, Vector4Variables, Vector2IntVariables, or Vector3IntVariables.")]
        [SerializeField] VectorReference m_vectorB;
        public VectorReference VectorB => m_vectorB;
    
        /// <value>Checks if the defined condition results in true or false.</value>
        public bool PassCondition
        {
            get
            {
                switch (m_variableType)
                {

                    case VariableType.Bool:
                    {
                        switch (m_comparison)
                        {
                            case BooleanComparisons.EqualTo:
                                return m_boolA.Value == m_boolB.Value;
                            case BooleanComparisons.NotEqualTo:
                                return m_boolA.Value != m_boolB.Value;
                            case BooleanComparisons.LessThan:
                                return (m_boolA.Value?1:0) < (m_boolB.Value?1:0);
                            case BooleanComparisons.LessThanOrEqualTo:
                                return true; 
                            case BooleanComparisons.GreaterThan:
                                return (m_boolA.Value?1:0) > (m_boolB.Value?1:0);
                            case BooleanComparisons.GreaterThanOrEqualTo:
                                return true;
                            default:
                                return false;
                        }
                    }

                    case VariableType.Number:
                    {
                        /*
                        Number comparisons of different types will cast to the type with the highest precision before doing comparison.
                        For example, if comparing an int A and a float B.
                        if (A > B)
                        would be treated as:
                        if ((float)(A)>B)
                        */

                        TypeCode highestPrecision = HighestPrecision(m_numberA.TypeCode, m_numberB.TypeCode);

                        switch(highestPrecision)
                        {
                            case TypeCode.Int32:
                                switch (m_comparison)
                                {
                                    case BooleanComparisons.EqualTo:
                                        return m_numberA.ValueInt == m_numberB.ValueInt;
                                    case BooleanComparisons.NotEqualTo:
                                        return m_numberA.ValueInt != m_numberB.ValueInt;
                                    case BooleanComparisons.LessThan:
                                        return m_numberA.ValueInt < m_numberB.ValueInt;
                                    case BooleanComparisons.LessThanOrEqualTo:
                                        return m_numberA.ValueInt <= m_numberB.ValueInt; 
                                    case BooleanComparisons.GreaterThan:
                                        return m_numberA.ValueInt > m_numberB.ValueInt; 
                                    case BooleanComparisons.GreaterThanOrEqualTo:
                                        return m_numberA.ValueInt >= m_numberB.ValueInt;
                                    default:
                                        return false;
                                }

                            default:
                            case TypeCode.Single:
                                switch (m_comparison)
                                {
                                    case BooleanComparisons.EqualTo:
                                        return m_numberA.ValueFloat == m_numberB.ValueFloat;
                                    case BooleanComparisons.NotEqualTo:
                                        return m_numberA.ValueFloat != m_numberB.ValueFloat;
                                    case BooleanComparisons.LessThan:
                                        return m_numberA.ValueFloat < m_numberB.ValueFloat;
                                    case BooleanComparisons.LessThanOrEqualTo:
                                        return m_numberA.ValueFloat <= m_numberB.ValueFloat; 
                                    case BooleanComparisons.GreaterThan:
                                        return m_numberA.ValueFloat > m_numberB.ValueFloat; 
                                    case BooleanComparisons.GreaterThanOrEqualTo:
                                        return m_numberA.ValueFloat >= m_numberB.ValueFloat;
                                    default:
                                        return false;
                                }

                            case TypeCode.Double:
                                switch (m_comparison)
                                {
                                    case BooleanComparisons.EqualTo:
                                        return m_numberA.ValueDouble == m_numberB.ValueDouble;
                                    case BooleanComparisons.NotEqualTo:
                                        return m_numberA.ValueDouble != m_numberB.ValueDouble;
                                    case BooleanComparisons.LessThan:
                                        return m_numberA.ValueDouble < m_numberB.ValueDouble;
                                    case BooleanComparisons.LessThanOrEqualTo:
                                        return m_numberA.ValueDouble <= m_numberB.ValueDouble; 
                                    case BooleanComparisons.GreaterThan:
                                        return m_numberA.ValueDouble > m_numberB.ValueDouble; 
                                    case BooleanComparisons.GreaterThanOrEqualTo:
                                        return m_numberA.ValueDouble >= m_numberB.ValueDouble;
                                    default:
                                        return false;
                                }
                        }
                    }

                    case VariableType.Vector:
                    {
                        // Get the larger of the two vectors length and decide whether we are comparing VectorInts
                        bool compareVectorInts = false;
                        int compareAtLength = HighestVectorPrecision(m_vectorA.VectorLength, m_vectorA.IsAVectorInt, m_vectorB.VectorLength, m_vectorB.IsAVectorInt, out compareVectorInts);

                        if (compareVectorInts)
                        {
                            // Int Vectors
                            switch (compareAtLength)
                            {
                                case 2:
                                    switch (m_comparison)
                                    {
                                        case BooleanComparisons.EqualTo:
                                            return m_vectorA.ValueVector2Int == m_vectorB.ValueVector2Int;
                                        case BooleanComparisons.NotEqualTo:
                                            return m_vectorA.ValueVector2Int != m_vectorB.ValueVector2Int;
                                        //  than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                        case BooleanComparisons.LessThan:
                                            return m_vectorA.ValueVector2Int.magnitude < m_vectorB.ValueVector2Int.magnitude;
                                        case BooleanComparisons.LessThanOrEqualTo:
                                            return m_vectorA.ValueVector2Int.magnitude <= m_vectorB.ValueVector2Int.magnitude; 
                                        case BooleanComparisons.GreaterThan:
                                            return m_vectorA.ValueVector2Int.magnitude > m_vectorB.ValueVector2Int.magnitude;
                                        case BooleanComparisons.GreaterThanOrEqualTo:
                                            return m_vectorA.ValueVector2Int.magnitude >= m_vectorB.ValueVector2Int.magnitude;
                                        default:
                                            return false;
                                    }

                                default:
                                case 3:
                                    switch (m_comparison)
                                    {
                                        case BooleanComparisons.EqualTo:
                                            return m_vectorA.ValueVector3Int == m_vectorB.ValueVector3Int;
                                        case BooleanComparisons.NotEqualTo:
                                            return m_vectorA.ValueVector3Int != m_vectorB.ValueVector3Int;
                                        // Less than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                        case BooleanComparisons.LessThan:
                                            return m_vectorA.ValueVector3Int.magnitude < m_vectorB.ValueVector3Int.magnitude;
                                        case BooleanComparisons.LessThanOrEqualTo:
                                            return m_vectorA.ValueVector3Int.magnitude <= m_vectorB.ValueVector3Int.magnitude; 
                                        case BooleanComparisons.GreaterThan:
                                            return m_vectorA.ValueVector3Int.magnitude > m_vectorB.ValueVector3Int.magnitude;
                                        case BooleanComparisons.GreaterThanOrEqualTo:
                                            return m_vectorA.ValueVector3Int.magnitude >= m_vectorB.ValueVector3Int.magnitude;
                                        default:
                                            return false;
                                    }
                            }
                        }
                        else
                        {
                            // Floating point Vectors
                            switch (compareAtLength)
                            {
                                case 2:
                                    switch (m_comparison)
                                    {
                                        case BooleanComparisons.EqualTo:
                                            return m_vectorA.ValueVector2 == m_vectorB.ValueVector2;
                                        case BooleanComparisons.NotEqualTo:
                                            return m_vectorA.ValueVector2 != m_vectorB.ValueVector2;
                                        // Less than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                        case BooleanComparisons.LessThan:
                                            return m_vectorA.ValueVector2.magnitude < m_vectorB.ValueVector2.magnitude;
                                        case BooleanComparisons.LessThanOrEqualTo:
                                            return  m_vectorA.ValueVector2.magnitude <= m_vectorB.ValueVector2.magnitude; 
                                        case BooleanComparisons.GreaterThan:
                                            return  m_vectorA.ValueVector2.magnitude > m_vectorB.ValueVector2.magnitude;
                                        case BooleanComparisons.GreaterThanOrEqualTo:
                                            return  m_vectorA.ValueVector2.magnitude >= m_vectorB.ValueVector2.magnitude;
                                        default:
                                            return false;
                                    }

                                default:
                                case 3:
                                    switch (m_comparison)
                                    {
                                        case BooleanComparisons.EqualTo:
                                            return m_vectorA.ValueVector3 == m_vectorB.ValueVector3;
                                        case BooleanComparisons.NotEqualTo:
                                            return m_vectorA.ValueVector3 != m_vectorB.ValueVector3;
                                        // Less than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                        case BooleanComparisons.LessThan:
                                            return m_vectorA.ValueVector3.magnitude < m_vectorB.ValueVector3.magnitude;
                                        case BooleanComparisons.LessThanOrEqualTo:
                                            return  m_vectorA.ValueVector3.magnitude <= m_vectorB.ValueVector3.magnitude; 
                                        case BooleanComparisons.GreaterThan:
                                            return  m_vectorA.ValueVector3.magnitude > m_vectorB.ValueVector3.magnitude;
                                        case BooleanComparisons.GreaterThanOrEqualTo:
                                            return  m_vectorA.ValueVector3.magnitude >= m_vectorB.ValueVector3.magnitude;
                                        default:
                                            return false;
                                    }

                                case 4:
                                    switch (m_comparison)
                                    {
                                        case BooleanComparisons.EqualTo:
                                            return m_vectorA.ValueVector4 == m_vectorB.ValueVector4;
                                        case BooleanComparisons.NotEqualTo:
                                            return m_vectorA.ValueVector4 != m_vectorB.ValueVector4;
                                        // Less than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                        case BooleanComparisons.LessThan:
                                            return m_vectorA.ValueVector4.magnitude < m_vectorB.ValueVector4.magnitude;
                                        case BooleanComparisons.LessThanOrEqualTo:
                                            return  m_vectorA.ValueVector4.magnitude <= m_vectorB.ValueVector4.magnitude; 
                                        case BooleanComparisons.GreaterThan:
                                            return  m_vectorA.ValueVector4.magnitude > m_vectorB.ValueVector4.magnitude;
                                        case BooleanComparisons.GreaterThanOrEqualTo:
                                            return  m_vectorA.ValueVector4.magnitude >= m_vectorB.ValueVector4.magnitude;
                                        default:
                                            return false;
                                    }
                            }
                        }
                    }

                    default:
                        Debug.LogError("Variable type not found in Condition switch case:" + m_variableType);
                        return false;
                }
            }
        }

        TypeCode HighestPrecision(TypeCode typeA, TypeCode typeB)
        {
            if (typeA == TypeCode.Double || typeB == TypeCode.Double)
                return TypeCode.Double;

            if (typeA == TypeCode.Single || typeB == TypeCode.Single)
                return TypeCode.Single;
                
            return TypeCode.Int32;
        }

        int HighestVectorPrecision(int lengthA, bool isAVectorIntA, int lengthB, bool isAVectorIntB, out bool isAVectorInt)
        {
            isAVectorInt = isAVectorIntA && isAVectorIntB; // Only use VectorInt if BOTH vectors are a vector int
            return Mathf.Max(lengthA, lengthB); // Return the longest length o the two vectors.
        }

        /// <summary>
        /// Validation method for Conditions. Asserts and logs if any suspicious variable fields are left null in the condition. Not performant. Should only be called in Editor.
        /// </summary>
        public void ValidateIntegrity(int traceLevel = 1)
        {
            #if UNITY_EDITOR
            
            // Get the call stack in case there are null values in the comparison
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();

            switch (m_variableType)
            {
                case VariableType.Bool:
                {
                    if (m_boolA.UseVariable)
                        Assert.IsNotNull(m_boolA, "m_boolA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    if (m_boolB.UseVariable)
                        Assert.IsNotNull(m_boolB, "m_boolB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    break;
                }

                case VariableType.Number:
                {
                    if (m_numberA.UseVariable)
                        Assert.IsNotNull(m_numberA, "m_numberA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    if (m_numberB.UseVariable)
                        Assert.IsNotNull(m_numberB, "m_numberB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    break;
                }

                case VariableType.Vector:
                {
                    if (m_vectorA.UseVariable)
                        Assert.IsNotNull(m_vectorA, "m_vectorA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    if (m_vectorB.UseVariable)
                        Assert.IsNotNull(m_vectorB, "m_vectorB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                    break;
                }

                default:
                    Debug.LogError("Variable type not found in Condition switch case:" + m_variableType);
                    break;
            }
            #endif
        }

        public static void ValidateIntegrity(ICollection<Condition> conditions)
        {
            #if UNITY_EDITOR
            foreach (Condition c in conditions)
                c.ValidateIntegrity();
            #endif
        }
        
#if UNITY_INCLUDE_TESTS
        public void SetValues(BoolReference boolA, BoolReference boolB, BooleanComparisons comparison)
        {
            m_variableType = VariableType.Bool;
            m_boolA = boolA;
            m_boolB = boolB;
            m_comparison = comparison;
        }
        
        public void SetValues(NumberReference numberA, NumberReference numberB, BooleanComparisons comparison)
        {
            m_variableType = VariableType.Number;
            m_numberA = numberA;
            m_numberB = numberB;
            m_comparison = comparison;
        }
        
        public void SetValues(VectorReference vectorA, VectorReference vectorB, BooleanComparisons comparison)
        {
            m_variableType = VariableType.Vector;
            m_vectorA = vectorA;
            m_vectorB = vectorB;
            m_comparison = comparison;
        }
#endif
    }
}
