using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
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