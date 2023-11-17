using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector2Reference
    {
        public bool UseConstant = true;
        public Vector2 ConstantValue;
        public Vector2Variable Variable;

        public Vector2Reference()
        { }

        public Vector2Reference(Vector2 value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Vector2 Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Vector2(Vector2Reference reference)
        {
            return reference.Value;
        }
    }
}