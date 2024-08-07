using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector2IntReference
    {
        public bool UseVariable = false;
        public Vector2Int ConstantValue;
        public Vector2IntVariable Variable;

        public Vector2IntReference()
        { }

        public Vector2IntReference(Vector2Int value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public Vector2IntReference(Vector2IntVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Vector2Int Value
            => UseVariable ? ((Vector2)Variable.Value).ToVector2Int() : ConstantValue;

        public static implicit operator Vector2Int(Vector2IntReference reference)
            => reference.Value;
    }
}