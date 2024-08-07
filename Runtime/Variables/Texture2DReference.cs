using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Texture2DReference
    {
        public bool UseVariable = false;
        public Texture2D ConstantValue;
        public Texture2DVariable Variable;

        public Texture2DReference()
        { }

        public Texture2DReference(Texture2D value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public Texture2DReference(Texture2DVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Texture2D Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Texture2D(Texture2DReference reference)
            => reference.Value;
    }
}
