using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class ColorReference
    {
        public bool UseConstant = true;
        public Color ConstantValue = Color.white;
        public ColorVariable Variable;

        public ColorReference()
        { }

        public ColorReference(Color value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Color Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Color(ColorReference reference)
        {
            return reference.Value;
        }
    }
}