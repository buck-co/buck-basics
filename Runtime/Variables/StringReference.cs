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

        public string Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator string(StringReference reference)
        {
            return reference.Value;
        }
    }
}