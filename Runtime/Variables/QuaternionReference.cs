// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

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
        
        public QuaternionReference(QuaternionVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Quaternion Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Quaternion(QuaternionReference reference)
            => reference.Value;
    }
}