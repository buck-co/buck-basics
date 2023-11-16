using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 7)]
    public class Vector4Variable : BaseScriptableObject
    {
        public Vector4 DefaultValue = Vector4.zero;
        
        private Vector4 currentValue;
        public Vector4 CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Vector4 value)
        {
            CurrentValue = value;
        }

        public void SetValue(Vector4Variable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}