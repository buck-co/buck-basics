using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable", order = 7)]
    public class Vector3Variable : BaseVariable
    {
        public Vector3 DefaultValue = Vector3.zero;
        
        private Vector3 m_currentValue;
        public Vector3 Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Vector3 value)
        {
            Value = value;
        }

        public void SetValue(Vector3Variable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}