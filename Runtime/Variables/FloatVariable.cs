using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : NumberVariable
    {
        public float DefaultValue;
        
        private float m_currentValue;
        public float Value
        {
            get { return m_currentValue; }
            set { 
                    m_currentValue = value; 
                    Clamp();
                    LogValueChange(m_currentValue.ToString());
                }
        }

        public void SetValue(float value)
        {
            Value = value;
        }

        public void SetValue(FloatVariable value)
        {
            Value = value.Value;
        }

        public void ApplyChange(float amount)
        {
            Value += amount;
        }

        public void ApplyChange(FloatVariable amount)
        {
            Value += amount.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
            Clamp();
        }

        public override System.TypeCode TypeCode => System.TypeCode.Single;


        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueFloat > Value)
            {
                m_currentValue = m_clampMin;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " min clamped to: " + m_currentValue.ToString());
                #endif
            }
            else
            if (m_clampToAMax && m_clampMax.ValueFloat < Value)
            {
                m_currentValue = m_clampMax;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
                #endif
            }
        }

        public override int ValueInt => (int)(Value);

        public override float ValueFloat => Value;

        public override double ValueDouble => (double)(Value);
     
    }
}