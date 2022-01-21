using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class QuaternionReference
    {
        public bool UseConstant = true;
        public Quaternion ConstantValue;
        public QuaternionVariable Variable;

        public QuaternionReference()
        { }

        public QuaternionReference(Quaternion value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Quaternion Value
        {
            get { return UseConstant ? ConstantValue : Variable.CurrentValue; }
        }

        public static implicit operator Quaternion(QuaternionReference reference)
        {
            return reference.Value;
        }
    }
}