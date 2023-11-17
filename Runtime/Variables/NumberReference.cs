using System;

namespace Buck
{
    [Serializable]
    public class NumberReference
    {
        public bool UseConstant = true;
        public float ConstantValue;
        public NumberVariable Variable;

        public NumberReference()
        { }

        public NumberReference(float value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public int ValueInt
        {
            get { return UseConstant ? (int)(ConstantValue) : Variable.ValueInt; }
        }

        public float ValueFloat
        {
            get { return UseConstant ? ConstantValue : Variable.ValueFloat; }
        }

        public double ValueDouble
        {
            get { return UseConstant ? (double)(ConstantValue) : Variable.ValueDouble; }
        }

        public System.TypeCode TypeCode
        {
            get { return UseConstant ? System.TypeCode.Single : Variable.TypeCode; }
        }

        public static implicit operator int(NumberReference reference)
        {
            return reference.ValueInt;
        }

        public static implicit operator float(NumberReference reference)
        {
            return reference.ValueFloat;
        }

        public static implicit operator double(NumberReference reference)
        {
            return reference.ValueDouble;
        }
    }
}