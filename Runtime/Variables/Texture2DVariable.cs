using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Texture2D Variable", order = 15)]
    public class Texture2DVariable : BaseScriptableObject
    {
        public Texture2D DefaultValue;
        
        private Texture2D currentValue;
        public Texture2D CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Texture2D value)
        {
            CurrentValue = value;
        }

        public void SetValue(Texture2DVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}