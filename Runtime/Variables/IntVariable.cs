using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Int Variable", order = 3)]
    public class IntVariable : BaseScriptableObject
    {
        public int DefaultValue = 0;
        
        private int currentValue;
        public int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
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
    }
}