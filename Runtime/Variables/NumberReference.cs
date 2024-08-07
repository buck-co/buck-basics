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
        
        public NumberReference(NumberVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public int ValueInt => UseVariable ? Variable.ValueInt : (int)ConstantValue;

        public float ValueFloat => UseVariable ? Variable.ValueFloat : ConstantValue;

        public double ValueDouble => UseVariable ? Variable.ValueDouble : (double)ConstantValue;

        public TypeCode TypeCode => UseVariable ? Variable.TypeCode : TypeCode.Single;

        public static implicit operator int(NumberReference reference)
            => reference.ValueInt;

        public static implicit operator float(NumberReference reference)
            => reference.ValueFloat;

        public static implicit operator double(NumberReference reference)
            => reference.ValueDouble;
    }
}