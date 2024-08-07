using System;

namespace Buck
{
    [Serializable]
    public class DoubleReference
    {
        public bool UseVariable = false;
        public double ConstantValue;
        public DoubleVariable Variable;

        public DoubleReference()
        { }

        public DoubleReference(double value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public DoubleReference(DoubleVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public double Value
            => UseVariable ? (double)Variable.Value : ConstantValue;

        public static implicit operator double(DoubleReference reference)
            => reference.Value;
    }
}
