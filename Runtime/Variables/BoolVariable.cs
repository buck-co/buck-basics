using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Bool Variable", order=2)]//Default unset order is 1 so list starts at 2
    public class BoolVariable : BaseVariable
    {

        protected override string DebugValue => (CurrentValue)?"true":"false";
        public bool DefaultValue = false;
        
        private bool currentValue;
        public bool CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; LogValueChange(); }
        }

        public void SetValue(bool value)
        {
            CurrentValue = value;
        }

        public void SetValue(BoolVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }


        //TODO: Is there a way to emit a warning if you write if (m_someBoolVariable == true) when you meant (m_someBoolVariable.CurrentValue == true)
        //Try Implicit Operators, try IComparable and IEquatable
    }
}