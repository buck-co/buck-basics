using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : NumberVariable
    {
        public double DefaultValue;
        
        private double m_currentValue;
        public double Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                Clamp();
                LogValueChange();
                }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(double value)
        {
            Value = value;
        }

        public void SetValue(DoubleVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(double amount)
        {
            Value += amount;
        }

        public void ApplyChange(DoubleVariable amount)
        {
            Value += amount.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }

        public override System.TypeCode TypeCode => System.TypeCode.Double;

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueDouble > Value)
                m_currentValue = m_clampMin;
            else
            if (m_clampToAMax && m_clampMax.ValueDouble < Value)
                m_currentValue = m_clampMax;
        }

        public override int ValueInt => (int)(Value);

        public override float ValueFloat => (float)(Value);

        public override double ValueDouble => (Value);
    }
}