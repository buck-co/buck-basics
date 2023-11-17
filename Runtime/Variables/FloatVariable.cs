using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : NumberVariable
    {
        public float DefaultValue;
        
        private float currentValue;
        public float CurrentValue
        {
            get { return currentValue; }
            set { 
                    currentValue = value; 
                    Clamp();
                }
        }

        public void SetValue(float value)
        {
            CurrentValue = value;
        }

        public void SetValue(FloatVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        public void ApplyChange(float amount)
        {
            CurrentValue += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            CurrentValue += amount.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
            Clamp();
        }

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueFloat > CurrentValue)
                currentValue = m_clampMin;
            else
            if (m_clampToAMax && m_clampMax.ValueFloat < CurrentValue)
                currentValue = m_clampMax;
        }

        public override int ToInt() => (int)(CurrentValue);

        public override float ToFloat() => CurrentValue;

        public override double ToDouble() => (double)(CurrentValue);
     
    }
}