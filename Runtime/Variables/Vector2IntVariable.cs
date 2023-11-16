using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable")]
    public class Vector2IntVariable : BaseScriptableObject
    {
        public Vector2Int DefaultValue = Vector2Int.zero;
        
        private Vector2Int currentValue;
        public Vector2Int CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Vector2Int value)
        {
            CurrentValue = value;
        }

        public void SetValue(Vector2IntVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}