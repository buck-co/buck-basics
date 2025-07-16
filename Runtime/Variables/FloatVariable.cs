// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Float Variable", order = 4)]
    public class FloatVariable : NumberVariable
    {
        public new float Value
        {
            get => ValueFloat;
            set
            {
                m_currentValue = value;
                Clamp();
                LogValueChange();
            }
        }

        public void ApplyChange(float amount)
            => Value += amount;

        public void ApplyChange(FloatVariable amount)
            => Value += amount.Value;

        public override TypeCode TypeCode
            => TypeCode.Single;

        protected override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueFloat > Value)
            {
                m_currentValue = m_clampMin.ValueFloat;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " min clamped to: " + ToString());
#endif
            }
            else if (m_clampToAMax && m_clampMax.ValueFloat < Value)
            {
                m_currentValue = m_clampMax.ValueFloat;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " max clamped to: " + ToString());
#endif
            }
        }
    }
}
