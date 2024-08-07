using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector2Reference
    {
        public bool UseVariable = false;
        public Vector2 ConstantValue;
        public Vector2Variable Variable;

        public Vector2Reference()
        { }

        public Vector2Reference(Vector2 value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public Vector2Reference(Vector2Variable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Vector2 Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Vector2(Vector2Reference reference)
            => reference.Value;
    }
}