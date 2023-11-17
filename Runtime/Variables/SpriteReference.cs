using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class SpriteReference
    {
        public bool UseConstant = true;
        public Sprite ConstantValue;
        public SpriteVariable Variable;

        public SpriteReference()
        { }

        public SpriteReference(Sprite value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Sprite Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Sprite(SpriteReference reference)
        {
            return reference.Value;
        }
    }
}