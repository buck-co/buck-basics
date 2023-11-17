using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : BaseVariable
    {
        public Vector2 DefaultValue = Vector2.zero;
        
        private Vector2 m_currentValue;
        public Vector2 Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange(m_currentValue.ToString());
                }
        }

        public void SetValue(Vector2 value)
        {
            Value = value;
        }

        public void SetValue(Vector2Variable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}