using UnityEngine;
using System;

namespace Buck
{
    public abstract class NumberVariable : BaseVariable<double>
    {
        [SerializeField] protected bool m_clampToAMin = false;
        public bool ClampToAMin => m_clampToAMin;
        
        [SerializeField] protected NumberReference m_clampMin;
        public NumberReference ClampMin => m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        public bool ClampToAMax => m_clampToAMax;
        
        [SerializeField] protected NumberReference m_clampMax;
        public NumberReference ClampMax => m_clampMax;
        
        public override string ToString()
            => Value.ToString();
        
        public override string ToString(string format, IFormatProvider formatProvider)
            => Value.ToString(format, formatProvider);

        public abstract TypeCode TypeCode { get; }
        
        protected abstract void Clamp();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            Clamp();
        }
        
        public int ValueInt
            => (int)m_currentValue;

        public float ValueFloat
            => (float)m_currentValue;

        public double ValueDouble
            => m_currentValue;
    }
}
