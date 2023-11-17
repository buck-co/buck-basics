using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector4Reference
    {
        public bool UseConstant = true;
        public Vector4 ConstantValue;
        public Vector4Variable Variable;

        public Vector4Reference()
        { }

        public Vector4Reference(Vector4 value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Vector4 Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator Vector4(Vector4Reference reference)
        {
            return reference.Value;
        }
    }
}