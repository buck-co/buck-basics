using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Bool Variable", order=2)]//Default unset order is 1 so list starts at 2
    public class BoolVariable : BaseScriptableObject
    {
        public bool DefaultValue = false;
        
        private bool currentValue;
        public bool CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(bool value)
        {
            CurrentValue = value;
        }

        public void SetValue(BoolVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}