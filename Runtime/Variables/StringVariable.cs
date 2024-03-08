using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/String Variable", order = 12)]
    public class StringVariable : BaseVariable
    {
        public string DefaultValue = "";
        
        string m_currentValue;
        public string Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ValueAsString
            => m_currentValue;

        public void SetValue(string value)
            => Value = value;

        public void SetValue(StringVariable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;
    }
}