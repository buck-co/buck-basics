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
            Int = 0, 
            Float = 1, 
            Bool = 2, 
            Vector3 = 3
            };

        [Tooltip("Use the dropdown to pick which type of Variable you wish to compare.")]
        [SerializeField] VariableType m_variableType = VariableType.Int;
        [SerializeField] IntReference m_intA;
        [SerializeField] FloatReference m_floatA;
        [SerializeField] BoolReference m_boolA;
        [SerializeField] Vector3Reference m_vector3A;

        [Tooltip("Use the dropdown to pick what kind of boolean comparison you want to use to compare the variables. Booleans will cast to 0 or 1 for size comparions. Vector3 will use magnitude.")]
        [SerializeField] BooleanComparisons m_comparison = BooleanComparisons.EqualTo;
        [SerializeField] IntReference m_intB;
        [SerializeField] FloatReference m_floatB;
        [SerializeField] BoolReference m_boolB;
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
                    case VariableType.Int:
                        {
                            switch (m_comparison)
                            {
                                case BooleanComparisons.EqualTo:
                                    return m_intA.Value == m_intB.Value;
                                case BooleanComparisons.NotEqualTo:
                                    return m_intA.Value != m_intB.Value;
                                case BooleanComparisons.LessThan:
                                    return m_intA.Value < m_intB.Value;
                                case BooleanComparisons.LessThanOrEqualTo:
                                    return m_intA.Value <= m_intB.Value; 
                                case BooleanComparisons.GreaterThan:
                                    return m_intA.Value > m_intB.Value; 
                                case BooleanComparisons.GreaterThanOrEqualTo:
                                    return m_intA.Value >= m_intB.Value;
                                default:
                                    return false;
                            }
                        }
                    
                    case VariableType.Float:
                        {
                            switch (m_comparison)
                            {
                                case BooleanComparisons.EqualTo:
                                    return m_floatA.Value == m_floatB.Value;
                                case BooleanComparisons.NotEqualTo:
                                    return m_floatA.Value != m_floatB.Value;
                                case BooleanComparisons.LessThan:
                                    return m_floatA.Value < m_floatB.Value;
                                case BooleanComparisons.LessThanOrEqualTo:
                                    return m_floatA.Value <= m_floatB.Value; 
                                case BooleanComparisons.GreaterThan:
                                    return m_floatA.Value > m_floatB.Value; 
                                case BooleanComparisons.GreaterThanOrEqualTo:
                                    return m_floatA.Value >= m_floatB.Value;
                                default:
                                    return false;
                            }
                        }
                    
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
                case VariableType.Int:
                    {
                        Assert.IsNotNull(m_intA, "m_intA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        Assert.IsNotNull(m_intB, "m_intB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        break;
                    }

                case VariableType.Float:
                    {
                        Assert.IsNotNull(m_floatA, "m_floatA is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
                        Assert.IsNotNull(m_floatB, "m_floatB is null in a comparison called by " + stackTrace.GetFrame(traceLevel).GetMethod().Name);
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
