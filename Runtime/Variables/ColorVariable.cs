using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Color Variable", order = 14)]
    public class ColorVariable : BaseVariable
    {
        public Color DefaultValue = Color.white;
        
        Color m_currentValue;
        public Color Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Color value)
            => Value = value;

        public void SetValue(ColorVariable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;
    }
}