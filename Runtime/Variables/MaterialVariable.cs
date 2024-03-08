using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Material Variable", order = 17)]
    public class MaterialVariable : BaseVariable
    {
        public Material DefaultValue;
        
        Material m_currentValue;
        public Material Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ValueAsString => m_currentValue != null ? m_currentValue.name : "null";

        public void SetValue(Material value)
            => Value = value;

        public void SetValue(MaterialVariable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;
    }
}