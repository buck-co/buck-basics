using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Quaternion Variable", order = 11)]
    public class QuaternionVariable : BaseVariable
    {
        public Quaternion DefaultValue = Quaternion.identity;
        
        private Quaternion m_currentValue;
        public Quaternion Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Quaternion value)
        {
            Value = value;
        }

        public void SetValue(QuaternionVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}