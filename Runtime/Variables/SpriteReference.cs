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

        public Sprite Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator Sprite(SpriteReference reference)
        {
            return reference.Value;
        }
    }
}