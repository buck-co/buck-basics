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
        
        public IntReference(IntVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public int Value
            => UseVariable ? (int)Variable.Value : ConstantValue;

        public static implicit operator int(IntReference reference)
            => reference.Value;
    }
}