using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : NumberVariable
    {
        public double DefaultValue;
        
        double m_currentValue;
        public double Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                Clamp();
                LogValueChange();
            }
        }
        
        public override string ValueAsString
            => m_currentValue.ToString();
        
        public override string ValueAsStringFormatted(string formatter)
            => m_currentValue.ToString(formatter);
        
        public override string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => m_currentValue.ToString(formatter, formatProvider);

        public void SetValue(double value)
            => Value = value;

        public void SetValue(DoubleVariable value)
            => Value = value.Value;

        public void ApplyChange(double amount)
            => Value += amount;

        public void ApplyChange(DoubleVariable amount)
            => Value += amount.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;

        public override TypeCode TypeCode
            => TypeCode.Double;

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueDouble > Value)
                m_currentValue = m_clampMin.ValueDouble;
            else
            if (m_clampToAMax && m_clampMax.ValueDouble < Value)
                m_currentValue = m_clampMax.ValueDouble;
        }

        public override int ValueInt
            => (int)(Value);

        public override float ValueFloat
            => (float)(Value);

        public override double ValueDouble
            => Value;
    }
}
