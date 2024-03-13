using System;

namespace Buck
{
    public interface NumberVariable
    {
        public TypeCode TypeCode { get; }
        public void Clamp();
        
        public int ValueInt { get; }

        public float ValueFloat { get; }

        public double ValueDouble { get; }

        public string ValueAsStringFormatted(string formatter);

        public string ValueAsStringFormatted(string formatter, IFormatProvider formatProvider);
        
        public void Raise();
    }
}
