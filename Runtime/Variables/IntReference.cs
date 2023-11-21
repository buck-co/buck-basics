using System;

namespace Buck
{
    [Serializable]
    public class IntReference
    {
        public bool UseVariable = false;
        public int ConstantValue;
        public IntVariable Variable;

        public IntReference()
        { }

        public IntReference(int value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public int Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator int(IntReference reference)
        {
            return reference.Value;
        }
    }
}