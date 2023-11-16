using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : BaseScriptableObject
    {
        public Vector2 DefaultValue = Vector2.zero;
        
        private Vector2 currentValue;
        public Vector2 CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Vector2 value)
        {
            CurrentValue = value;
        }

        public void SetValue(Vector2Variable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}