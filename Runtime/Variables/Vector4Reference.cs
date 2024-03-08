using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector4Reference
    {
        public bool UseVariable = false;
        public Vector4 ConstantValue;
        public Vector4Variable Variable;

        public Vector4Reference()
        { }

        public Vector4Reference(Vector4 value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public Vector4 Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Vector4(Vector4Reference reference)
            => reference.Value;
    }
}
