using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector3IntReference
    {
        public bool UseConstant = true;
        public Vector3Int ConstantValue;
        public Vector3IntVariable Variable;

        public Vector3IntReference()
        { }

        public Vector3IntReference(Vector3Int value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Vector3Int Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Vector3Int(Vector3IntReference reference)
        {
            return reference.Value;
        }
    }
}