using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class VectorReference
    {
        public bool UseVariable = false;
        public Vector4 ConstantValue;
        public VectorVariable Variable;

        public VectorReference()
        { }

        public VectorReference(Vector4 value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public VectorReference(VectorVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public int VectorLength
            => UseVariable ? Variable.VectorLength : 4;

        public bool IsAVectorInt
            => UseVariable ? Variable.IsAVectorInt : false;

        public Vector2 ValueVector2
            => UseVariable ? Variable.ValueVector2 : (Vector2)(ConstantValue);

        public Vector3 ValueVector3
            => UseVariable ? Variable.ValueVector3 : (Vector3)(ConstantValue);

        public Vector4 ValueVector4
            => UseVariable ? Variable.ValueVector4 : (Vector4)(ConstantValue);

        public Vector2Int ValueVector2Int
            => UseVariable ? Variable.ValueVector2Int:((Vector2)(ConstantValue)).ToVector2Int();

        public Vector3Int ValueVector3Int
            => UseVariable ? Variable.ValueVector3Int:((Vector3)(ConstantValue)).ToVector3Int();

        public static implicit operator Vector2(VectorReference reference)
            => reference.ValueVector2;

        public static implicit operator Vector3(VectorReference reference)
            => reference.ValueVector3;

        public static implicit operator Vector4(VectorReference reference)
            => reference.ValueVector4;

        public static implicit operator Vector2Int(VectorReference reference)
            => reference.ValueVector2Int;

        public static implicit operator Vector3Int(VectorReference reference)
            => reference.ValueVector3Int;
    }
}
