using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Texture2DReference
    {
        public bool UseConstant = true;
        public Texture2D ConstantValue;
        public Texture2DVariable Variable;

        public Texture2DReference()
        { }

        public Texture2DReference(Texture2D value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Texture2D Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Texture2D(Texture2DReference reference)
        {
            return reference.Value;
        }
    }
}