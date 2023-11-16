using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/String Variable", order = 11)]
    public class StringVariable : BaseScriptableObject
    {
        public string DefaultValue = "";
        
        private string currentValue;
        public string CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(string value)
        {
            CurrentValue = value;
        }

        public void SetValue(StringVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}