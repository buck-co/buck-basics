using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Int Variable", order = 3)]
    public class IntVariable : NumberVariable
    {
        public int DefaultValue = 0;
        
        private int m_currentValue;
        public int Value
        {
            get { return m_currentValue; }
            set { 
                    m_currentValue = value;
                    Clamp();
                    LogValueChange();
                }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(int value)
        {
            Value = value;
        }

        public void SetValue(IntVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(int amount)
        {
            Value += amount;
        }

        public void ApplyChange(IntVariable amount)
        {
            Value += amount.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }

        public override System.TypeCode TypeCode => System.TypeCode.Int32;

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueInt > Value)
            {
                m_currentValue = m_clampMin.ValueInt;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " min clamped to: " + m_currentValue.ToString());
                #endif
            }
            else
            if (m_clampToAMax && m_clampMax.ValueInt < Value)
            {
                m_currentValue = m_clampMax.ValueInt;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
                #endif
            }
        }

        public override int ValueInt => Value;

        public override float ValueFloat => (float)(Value);

        public override double ValueDouble => (double)(Value);

    }
}