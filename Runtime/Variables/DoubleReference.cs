using System;

namespace Buck
{
    [Serializable]
    public class DoubleReference
    {
        public bool UseConstant = true;
        public double ConstantValue;
        public DoubleVariable Variable;

        public DoubleReference()
        { }

        public DoubleReference(double value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public double Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator double(DoubleReference reference)
        {
            return reference.Value;
        }
    }
}