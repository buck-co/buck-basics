using System;

namespace Buck
{
    [Serializable]
    public class FloatReference
    {
        public bool UseVariable = false;
        public float ConstantValue;
        public FloatVariable Variable;

        public FloatReference()
        { }

        public FloatReference(float value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public float Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator float(FloatReference reference)
        {
            return reference.Value;
        }
    }
}