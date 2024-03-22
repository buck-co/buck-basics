using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : NumberVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;
        
        public override string ValueAsStringFormatted(string formatter)
            => ((float)m_currentValue).ToString(formatter);
        
        public override string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => ((float)m_currentValue).ToString(formatter, formatProvider);

        public void SetValue(FloatVariable value)
            => Value = value.Value;

        public void ApplyChange(float amount)
            => Value += (decimal)amount;

        public void ApplyChange(FloatVariable amount)
            => Value += amount.Value;

        protected override void OnEnable()
        {
            m_currentValue = DefaultValue;
            Clamp();
        }

        public override TypeCode TypeCode
            => TypeCode.Single;

        public override void Clamp()
        {
            if (m_clampToAMin && (decimal)m_clampMin.ValueFloat > Value)
            {
                m_currentValue = (decimal)m_clampMin.ValueFloat;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " min clamped to: " + m_currentValue.ToString());
                #endif
            }
            else
            if (m_clampToAMax && (decimal)m_clampMax.ValueFloat < Value)
            {
                m_currentValue = (decimal)m_clampMax.ValueFloat;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
#endif
            }
        }

        public override int ValueInt
            => (int)(Value);

        public override float ValueFloat
            => (float)Value;

        public override double ValueDouble
            => (double)(Value);
    }
}
