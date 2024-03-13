using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : BaseVariable<float>, NumberVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;
        
        public string ValueAsStringFormatted(string formatter)
            => m_currentValue.ToString(formatter);
        
        public string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => m_currentValue.ToString(formatter, formatProvider);

        public void SetValue(FloatVariable value)
            => Value = value.Value;

        public void ApplyChange(float amount)
            => Value += amount;

        public void ApplyChange(FloatVariable amount)
            => Value += amount.Value;

        protected override void OnEnable()
        {
            m_currentValue = DefaultValue;
            Clamp();
        }

        public TypeCode TypeCode
            => TypeCode.Single;

        public void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueFloat > Value)
            {
                m_currentValue = m_clampMin.ValueFloat;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " min clamped to: " + m_currentValue.ToString());
                #endif
            }
            else
            if (m_clampToAMax && m_clampMax.ValueFloat < Value)
            {
                m_currentValue = m_clampMax.ValueFloat;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
#endif
            }
        }

        public int ValueInt
            => (int)(Value);

        public float ValueFloat
            => Value;

        public double ValueDouble
            => (double)(Value);
    }
}
