using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Quaternion Variable")]
    public class QuaternionVariable : BaseScriptableObject
    {
        public Quaternion DefaultValue = Quaternion.identity;
        
        private Quaternion currentValue;
        public Quaternion CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Quaternion value)
        {
            CurrentValue = value;
        }

        public void SetValue(QuaternionVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}