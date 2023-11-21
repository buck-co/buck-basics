using System;

namespace Buck
{
    [Serializable]
    public class BoolReference
    {
        public bool UseVariable = false;
        public bool ConstantValue;
        public BoolVariable Variable;

        public BoolReference()
        { }

        public BoolReference(bool value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public bool Value
        {
            get { return UseVariable ?  Variable.Value : ConstantValue; }
        }

        public static implicit operator bool(BoolReference reference)
        {
            return reference.Value;
        }
    }
}