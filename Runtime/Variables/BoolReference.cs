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
        
        public BoolReference(BoolVariable variable)
        {
            UseVariable = true;
            Variable = variable;
        }

        public bool Value
            => UseVariable ?  Variable.Value : ConstantValue;

        public static implicit operator bool(BoolReference reference)
            => reference.Value;
    }
}