using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class SpriteReference
    {
        public bool UseVariable = false;
        public Sprite ConstantValue;
        public SpriteVariable Variable;

        public SpriteReference()
        { }

        public SpriteReference(Sprite value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public SpriteReference(SpriteVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Sprite Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Sprite(SpriteReference reference)
            => reference.Value;
    }
}