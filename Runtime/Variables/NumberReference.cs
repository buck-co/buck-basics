using System;

namespace Buck
{
    [Serializable]
    public class NumberReference
    {
        public bool UseVariable = false;
        public float ConstantValue;
        public NumberVariable Variable;

        public NumberReference()
        { }

        public NumberReference(float value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public int ValueInt
        {
            get { return UseVariable ? Variable.ValueInt:(int)(ConstantValue); }
        }

        public float ValueFloat
        {
            get { return UseVariable ? Variable.ValueFloat:ConstantValue; }
        }

        public double ValueDouble
        {
            get { return UseVariable ? Variable.ValueDouble:(double)(ConstantValue); }
        }

        public System.TypeCode TypeCode
        {
            get { return UseVariable ? Variable.TypeCode:System.TypeCode.Single; }
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