using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : BaseScriptableObject
    {
        public Vector3Int DefaultValue = Vector3Int.zero;
        
        private Vector3Int currentValue;
        public Vector3Int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Vector3Int value)
        {
            CurrentValue = value;
        }

        public void SetValue(Vector3IntVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}