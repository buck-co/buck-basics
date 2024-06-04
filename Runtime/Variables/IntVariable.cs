using UnityEngine;
using System;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Int Variable", order = 3)]
    public class IntVariable : NumberVariable
    {
        public new int Value
        {
            get => ValueInt;
            set
            {
                m_currentValue = value;
                Clamp();
                LogValueChange();
            }
        }

        public void ApplyChange(int amount)
            => Value += amount;

        public void ApplyChange(IntVariable amount)
            => Value += amount.Value;

        public override TypeCode TypeCode
            => TypeCode.Int32;

        protected override void Clamp()
        {
            if (m_clampToAMin && m_clampMin.ValueInt > Value)
            {
                m_currentValue = m_clampMin.ValueInt;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " min clamped to: " + ToString());
#endif
            }
            else if (m_clampToAMax && m_clampMax.ValueInt < Value)
            {
                m_currentValue = m_clampMax.ValueInt;
                
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                if (m_debugChanges)
                    Debug.Log("Value of " + name + " max clamped to: " + ToString());
#endif
            }
        }

    }
}
