using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 8)]
    public class Vector4Variable : BaseVariable
    {
        public Vector4 DefaultValue = Vector4.zero;
        
        private Vector4 m_currentValue;
        public Vector4 Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange(m_currentValue.ToString());
                }
        }

        public void SetValue(Vector4 value)
        {
            Value = value;
        }

        public void SetValue(Vector4Variable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}