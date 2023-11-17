using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : NumberVariable
    {
        public double DefaultValue;
        
        private double currentValue;
        public double CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(double value)
        {
            CurrentValue = value;
        }

        public void SetValue(DoubleVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        public void ApplyChange(double amount)
        {
            CurrentValue += amount;
        }

        public void ApplyChange(DoubleVariable amount)
        {
            CurrentValue += amount.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueDouble > CurrentValue)
                currentValue = m_clampMin;
            else
            if (m_clampToAMax && m_clampMax.ValueDouble < CurrentValue)
                currentValue = m_clampMax;
        }

        public override int ToInt() => (int)(CurrentValue);

        public override float ToFloat() => (float)(CurrentValue);

        public override double ToDouble() => (CurrentValue);
    }
}