using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class ColorReference
    {
        public bool UseVariable = false;
        public Color ConstantValue = Color.white;
        public ColorVariable Variable;

        public ColorReference()
        { }

        public ColorReference(Color value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public Color Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator Color(ColorReference reference)
        {
            return reference.Value;
        }
    }
}