using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : NumberVariable
    {
        public float DefaultValue;
        
        float m_currentValue;
        public float Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value; 
                Clamp();
                LogValueChange();
            }
        }
        
        public override string ValueAsString
            => m_currentValue.ToString();

        public override string ValueAsStringFormatted(string formatter)
            => m_currentValue.ToString(formatter);
        
        public override string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => m_currentValue.ToString(formatter, formatProvider);

        public void SetValue(float value)
            => Value = value;

        public void SetValue(FloatVariable value)
            => Value = value.Value;

        public void ApplyChange(float amount)
            => Value += amount;

        public void ApplyChange(FloatVariable amount)
            => Value += amount.Value;

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
            Clamp();
        }

        public override TypeCode TypeCode
            => TypeCode.Single;

        public override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueFloat > Value)
            {
                m_currentValue = m_clampMin.ValueFloat;
                
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " min clamped to: " + m_currentValue.ToString());
                #endif
            }
            else
            if (m_clampToAMax && m_clampMax.ValueFloat < Value)
            {
                m_currentValue = m_clampMax.ValueFloat;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                    if (m_debugChanges)
                        Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
#endif
            }
        }

        public override int ValueInt
            => (int)(Value);

        public override float ValueFloat
            => Value;

        public override double ValueDouble
            => (double)(Value);
    }
}
