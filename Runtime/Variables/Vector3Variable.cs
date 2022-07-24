using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable")]
    public class Vector3Variable : BaseScriptableObject
    {
        public Vector3 DefaultValue = Vector3.zero;
        
        private Vector3 currentValue;
        public Vector3 CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Vector3 value)
        {
            CurrentValue = value;
        }

        public void SetValue(Vector3Variable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}