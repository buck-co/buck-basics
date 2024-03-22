using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : NumberVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;
        
        public override string ValueAsStringFormatted(string formatter)
            => ((double)m_currentValue).ToString(formatter);
        
        public override string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => ((double)m_currentValue).ToString(formatter, formatProvider);

        public void SetValue(DoubleVariable value)
            => Value = value.Value;

        public void ApplyChange(double amount)
            => Value += (decimal)amount;

        public void ApplyChange(DoubleVariable amount)
            => Value += amount.Value;

        public override TypeCode TypeCode
            => TypeCode.Double;

        public override void Clamp()
        {
            if (m_clampToAMin && (decimal)m_clampMin.ValueDouble > Value)
                m_currentValue = (decimal)m_clampMin.ValueDouble;
            else if (m_clampToAMax && (decimal)m_clampMax.ValueDouble < Value)
                m_currentValue = (decimal)m_clampMax.ValueDouble;
        }

        public override int ValueInt
            => (int)Value;

        public override float ValueFloat
            => (float)Value;

        public override double ValueDouble
            => (double)Value;
    }
}
