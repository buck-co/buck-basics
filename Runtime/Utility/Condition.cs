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
        /// <param name="conditions"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        static public bool PassConditions(this ICollection<Condition> conditions)
        {
            if (conditions == null)
                return true;

          
            foreach(Condition c in conditions)
            {
                if (!c.PassCondition)
                    return false;
            }
            

            return true;
        }
              

        /// <summary>
        /// Loops through a array of Conditions and returns true if they all pass, false if any of them fail.
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns> <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <returns></returns>
        static public bool PassConditions(this Condition[] conditions)
        {
            if (conditions == null)
                return true;

          
            foreach(Condition c in conditions)
            {
                if (!c.PassCondition)
                    return false;
            }
            

            return true;
        }
    }

    
    /// <summary>
    /// A serializable class that can be used to define basic boolean conditional logic comparing two variables. 
    /// Supports Int, Bool, Float, and Vector3. Uses Buck Basic VariableReferences to support optionally referencing Buck Basics Scriptable Object Variables.
    /// See script for usage examples./// 
    /// </summary>
    
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

    

    
    [Serializable]
    public class Condition
    {
        public enum BooleanComparisons{EqualTo, NotEqualTo, LessThan, LessThanOrEqualTo, GreaterThan, GreaterThanOrEqualTo}
        public enum VariableType{
            Bool = 0, 
            Number = 1, 
            Vector3 = 2
            };

        [Tooltip("Use the dropdown to pick which type of Variable you wish to compare.")]
        [SerializeField] VariableType m_variableType = VariableType.Bool;
        [SerializeField] BoolReference m_boolA;
        [SerializeField] NumberReference m_numberA;
        [SerializeField] Vector3Reference m_vector3A;

        [Tooltip("Use the dropdown to pick what kind of boolean comparison you want to use to compare the variables. Booleans will cast to 0 or 1 for size comparions. Vector3 will use magnitude.")]
        [SerializeField] BooleanComparisons m_comparison = BooleanComparisons.EqualTo;
        [SerializeField] BoolReference m_boolB;
        [SerializeField] NumberReference m_numberB;
        [SerializeField] Vector3Reference m_vector3B;
        
        /// <summary>
        /// Checks if the defined condition results in true or false.
        /// </summary> <summary>
        /// 
        /// </summary>
        /// <value></value>
        public bool PassCondition
        {
            get
            {
                #if UNITY_EDITOR
                //ValidateIntegrity(2);
                #endif

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
                                    return ((m_boolA.Value)?1:0) < ((m_boolB.Value)?1:0);
                                case BooleanComparisons.LessThanOrEqualTo:
                                    return true; 
                                case BooleanComparisons.GreaterThan:
                                    return ((m_boolA.Value)?1:0) > ((m_boolB.Value)?1:0);
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

                            System.TypeCode highestPrecision = HighestPrecision(m_numberA.TypeCode, m_numberB.TypeCode);
                            switch(highestPrecision)
                            {
                                case System.TypeCode.Int32:
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
                                case System.TypeCode.Single:
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

                                case System.TypeCode.Double:
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

                    case VariableType.Vector3:
                        {
                            switch (m_comparison)
                            {
                                case BooleanComparisons.EqualTo:
                                    return m_vector3A.Value == m_vector3B.Value;
                                case BooleanComparisons.NotEqualTo:
                                    return m_vector3A.Value != m_vector3B.Value;
                                //Less than and greater than comparisons for vectors use magnitudes (kind of weird, but could be useful at some point?)
                                case BooleanComparisons.LessThan:
                                    return m_vector3A.Value.magnitude < m_vector3B.Value.magnitude;
                                case BooleanComparisons.LessThanOrEqualTo:
                                    return  m_vector3A.Value.magnitude <= m_vector3B.Value.magnitude; 
                                case BooleanComparisons.GreaterThan:
                                    return  m_vector3A.Value.magnitude > m_vector3B.Value.magnitude;
                                case BooleanComparisons.GreaterThanOrEqualTo:
                                    return  m_vector3A.Value.magnitude >= m_vector3B.Value.magnitude;
                                default:
                                    return false;
                            }
                        }
                    


                    default:
                        Debug.LogError("Variable type not found in Condition switch case:" + m_variableType);
                        return false;
                }
            }
        }

        System.TypeCode HighestPrecision(System.TypeCode typeA, System.TypeCode typeB)
        {
            if (typeA == System.TypeCode.Double || typeB == System.TypeCode.Double)
                return System.TypeCode.Double;

            if (typeA == System.TypeCode.Single || typeB == System.TypeCode.Single)
                return System.TypeCode.Single;
                
            return System.TypeCode.Int32;
        }




        /// <summary>
        /// Validation method for Conditions. Asserts and logs if any supsicious variable fields are left null in the condition. Not performant. Should only be called in Editor.
        /// </summary>
        /// <param name="traceLevel"></param>
        public void ValidateIntegrity(int traceLevel = 1)
        {
            #if UNITY_EDITOR
            // Get the call stack in case there are null values in the comparison
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();

            switch (m_variableType)
            {
                case VariableType.Number:
                    {
                        Assert.IsNotNull(m_numberA, "m_numberA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        Assert.IsNotNull(m_numberB, "m_numberB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        break;
                    }

                case VariableType.Bool:
                    {
                        Assert.IsNotNull(m_boolA, "m_boolA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        Assert.IsNotNull(m_boolB, "m_boolB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        break;
                    }

                case VariableType.Vector3:
                    {
                        Assert.IsNotNull(m_vector3A, "m_vector3A is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        Assert.IsNotNull(m_vector3B, "m_vector3B is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
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
    }
}
