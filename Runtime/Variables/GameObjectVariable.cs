using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseScriptableObject
    {
        public GameObject DefaultValue;
        
        private GameObject currentValue;
        public GameObject CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(GameObject value)
        {
            CurrentValue = value;
        }

        public void SetValue(GameObjectVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}