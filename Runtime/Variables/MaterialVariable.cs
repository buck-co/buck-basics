using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Material Variable", order = 17)]
    public class MaterialVariable : BaseVariable
    {
        public Material DefaultValue;
        
        private Material m_currentValue;
        public Material Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange((m_currentValue != null)?m_currentValue.name:"null");
            }
        }

        public void SetValue(Material value)
        {
            Value = value;
        }

        public void SetValue(MaterialVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}