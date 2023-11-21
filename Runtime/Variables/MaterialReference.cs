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

        public Material Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator Material(MaterialReference reference)
        {
            return reference.Value;
        }
    }
}