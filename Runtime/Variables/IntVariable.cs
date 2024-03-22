using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Int Variable", order = 3)]
    public class IntVariable : NumberVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;
        
        public override string ValueAsString
            => ((int)m_currentValue).ToString();

        public override string ValueAsStringFormatted(string formatter)
            => ((int)m_currentValue).ToString(formatter);
        
        public override string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider)
            => ((int)m_currentValue).ToString(formatter, formatProvider);

        public void SetValue(IntVariable value)
            => Value = value.Value;

        public void ApplyChange(int amount)
            => Value += amount;

        public void ApplyChange(IntVariable amount)
            => Value += amount.Value;

        public override TypeCode TypeCode
            => TypeCode.Int32;

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
            else if (m_clampToAMax && m_clampMax.ValueInt < Value)
            {
                m_currentValue = m_clampMax.ValueInt;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " max clamped to: " + m_currentValue.ToString());
#endif
            }
        }

        public override int ValueInt
            => (int)Value;

        public override float ValueFloat
            => (float)(Value);

        public override double ValueDouble
            => (double)(Value);
    }
}
