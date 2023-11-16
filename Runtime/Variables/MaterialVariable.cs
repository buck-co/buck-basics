using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Material Variable", order = 17)]
    public class MaterialVariable : BaseScriptableObject
    {
        public Material DefaultValue;
        
        private Material currentValue;
        public Material CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Material value)
        {
            CurrentValue = value;
        }

        public void SetValue(MaterialVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}