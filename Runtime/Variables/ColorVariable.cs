using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Color Variable", order = 14)]
    public class ColorVariable : BaseVariable
    {
        public Color DefaultValue = Color.white;
        
        private Color m_currentValue;
        public Color Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange(m_currentValue.ToString());
                }
        }

        public void SetValue(Color value)
        {
            Value = value;
        }

        public void SetValue(ColorVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}