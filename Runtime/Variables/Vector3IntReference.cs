using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector3IntReference
    {
        public bool UseVariable = false;
        public Vector3Int ConstantValue;
        public Vector3IntVariable Variable;

        public Vector3IntReference()
        { }

        public Vector3IntReference(Vector3Int value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public Vector3Int Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator Vector3Int(Vector3IntReference reference)
        {
            return reference.Value;
        }
    }
}