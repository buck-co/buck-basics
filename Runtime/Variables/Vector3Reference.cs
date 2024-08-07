using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class Vector3Reference
    {
        public bool UseVariable = false;
        public Vector3 ConstantValue;
        public Vector3Variable Variable;

        public Vector3Reference()
        { }

        public Vector3Reference(Vector3 value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public Vector3Reference(Vector3Variable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Vector3 Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Vector3(Vector3Reference reference)
            => reference.Value;
    }
}
