using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : BaseVariable
    {
        public Vector3Int DefaultValue = Vector3Int.zero;
        
        private Vector3Int m_currentValue;
        public Vector3Int Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Vector3Int value)
        {
            Value = value;
        }

        public void SetValue(Vector3IntVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}