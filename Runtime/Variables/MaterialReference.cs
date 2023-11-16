using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class MaterialReference
    {
        public bool UseConstant = true;
        public Material ConstantValue;
        public MaterialVariable Variable;

        public MaterialReference()
        { }

        public MaterialReference(Material value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public Material Value
        {
            get { return UseConstant ? ConstantValue : Variable.CurrentValue; }
        }

        public static implicit operator Material(MaterialReference reference)
        {
            return reference.Value;
        }
    }
}