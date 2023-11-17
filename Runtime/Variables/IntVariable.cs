using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Int Variable", order = 3)]
    public class IntVariable : NumberVariable
    {
        public int DefaultValue = 0;
        
        private int currentValue;
        public int CurrentValue
        {
            get { return currentValue; }
            set { 
                    currentValue = value;
                    Clamp();
                }
        }

        public void SetValue(int value)
        {
            CurrentValue = value;
        }

        public void SetValue(IntVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        public void ApplyChange(int amount)
        {
            CurrentValue += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            CurrentValue += amount.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueInt > CurrentValue)
                currentValue = m_clampMin;
            else
            if (m_clampToAMax && m_clampMax.ValueInt < CurrentValue)
                currentValue = m_clampMax;
        }

        public override int ToInt() => CurrentValue;

        public override float ToFloat() => (float)(CurrentValue);

        public override double ToDouble() => (double)(CurrentValue);

    }
}