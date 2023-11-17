using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Texture2D Variable", order = 15)]
    public class Texture2DVariable : BaseVariable
    {
        public Texture2D DefaultValue;
        
        private Texture2D m_currentValue;
        public Texture2D Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        
        public override string ValueAsString => (m_currentValue != null)?m_currentValue.name:"null";

        public void SetValue(Texture2D value)
        {
            Value = value;
        }

        public void SetValue(Texture2DVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}