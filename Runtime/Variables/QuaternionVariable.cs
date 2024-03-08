using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Quaternion Variable", order = 11)]
    public class QuaternionVariable : BaseVariable
    {
        public Quaternion DefaultValue = Quaternion.identity;
        
        Quaternion m_currentValue;
        public Quaternion Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        public override string ValueAsString
            => m_currentValue.ToString();

        public void SetValue(Quaternion value)
            => Value = value;

        public void SetValue(QuaternionVariable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;
    }
}
