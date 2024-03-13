using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : BaseVariable<double>, NumberVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;
        
        public string ValueAsStringFormatted(string formatter)
            => m_currentValue.ToString(formatter);
        
        public string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => m_currentValue.ToString(formatter, formatProvider);

        public void SetValue(DoubleVariable value)
            => Value = value.Value;

        public void ApplyChange(double amount)
            => Value += amount;

        public void ApplyChange(DoubleVariable amount)
            => Value += amount.Value;

        public TypeCode TypeCode
            => TypeCode.Double;

        public void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueDouble > Value)
                m_currentValue = m_clampMin.ValueDouble;
            else if (m_clampToAMax && m_clampMax.ValueDouble < Value)
                m_currentValue = m_clampMax.ValueDouble;
        }

        public int ValueInt
            => (int)Value;

        public float ValueFloat
            => (float)Value;

        public double ValueDouble
            => Value;
    }
}
