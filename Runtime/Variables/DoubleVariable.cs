using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : BaseScriptableObject
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
    }
}