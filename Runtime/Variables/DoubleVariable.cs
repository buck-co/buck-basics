// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Double Variable", order = 5)]
    public class DoubleVariable : NumberVariable
    {
        public new double Value
        {
            get => ValueDouble;
            set
            {
                m_currentValue = value;
                Clamp();
                LogValueChange();
            }
        }

        public void ApplyChange(double amount)
            => Value += amount;

        public void ApplyChange(DoubleVariable amount)
            => Value += amount.Value;

        public override TypeCode TypeCode
            => TypeCode.Double;
        
        protected override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueDouble > Value)
            {
                m_currentValue = m_clampMin.ValueDouble;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " min clamped to: " + ToString());
#endif
            }
            else if (m_clampToAMax && m_clampMax.ValueDouble < Value)
            {
                m_currentValue = m_clampMax.ValueDouble;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " max clamped to: " + ToString());
#endif
            }
        }
    }
}
