using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class QuaternionReference
    {
        public bool UseVariable = false;
        public Quaternion ConstantValue;
        public QuaternionVariable Variable;

        public QuaternionReference()
        { }

        public QuaternionReference(Quaternion value)
        {
            UseVariable = false;
            ConstantValue = value;
        }

        public Quaternion Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator Quaternion(QuaternionReference reference)
        {
            return reference.Value;
        }
    }
}