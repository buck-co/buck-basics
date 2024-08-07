using System;

namespace Buck
{
    [Serializable]
    public class StringReference
    {
        public bool UseVariable = false;
        public string ConstantValue;
        public StringVariable Variable;

        public StringReference()
        { }

        public StringReference(string value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public StringReference(StringVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public string Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator string(StringReference reference)
            => reference.Value;
    }
}