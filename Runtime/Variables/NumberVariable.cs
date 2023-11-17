using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buck
{

    public abstract class NumberVariable:BaseVariable
    {
        [SerializeField] protected bool m_clampToAMin = false;
        
        [SerializeField] protected NumberReference m_clampMin;

        [SerializeField] protected bool m_clampToAMax = false;
        
        [SerializeField] protected NumberReference m_clampMax;

        public abstract System.TypeCode TypeCode{get;}
        public abstract void Clamp();
        
        public abstract int ValueInt{get;}

        public abstract float ValueFloat{get;}

        public abstract double ValueDouble{get;}


        
        
    }
}
