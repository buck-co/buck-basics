// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

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
        
        public FloatReference(FloatVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public float Value
            => UseVariable ? (float)Variable.Value : ConstantValue;

        public static implicit operator float(FloatReference reference)
            => reference.Value;
    }
}
