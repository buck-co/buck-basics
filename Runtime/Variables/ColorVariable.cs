using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Color Variable", order = 12)]
    public class ColorVariable : BaseScriptableObject
    {
        public Color DefaultValue = Color.white;
        
        private Color currentValue;
        public Color CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Color value)
        {
            CurrentValue = value;
        }

        public void SetValue(ColorVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}