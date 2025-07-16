// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class MaterialReference
    {
        public bool UseVariable = false;
        public Material ConstantValue;
        public MaterialVariable Variable;

        public MaterialReference()
        { }

        public MaterialReference(Material value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public MaterialReference(MaterialVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Material Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Material(MaterialReference reference)
            => reference.Value;
    }
}