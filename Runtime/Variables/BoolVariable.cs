using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Bool Variable", order=2)]//Default unset order is 1 so list starts at 2
    public class BoolVariable : BaseVariable
    {

        public bool DefaultValue = false;
        
        private bool m_currentValue;
        public bool Value
        {
            get { return m_currentValue; }
            set { m_currentValue = value; LogValueChange((Value)?"true":"false"); }
        }

        public void SetValue(bool value)
        {
            Value = value;
        }

        public void SetValue(BoolVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }


        //TODO: Is there a way to emit a warning if you write if (m_someBoolVariable == true) when you meant (m_someBoolVariable.CurrentValue == true)
        //Try Implicit Operators, try IComparable and IEquatable
    }
}