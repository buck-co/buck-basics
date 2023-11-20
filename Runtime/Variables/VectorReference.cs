using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class VectorReference
    {
        public bool UseConstant = true;
        public Vector4 ConstantValue;
        public VectorVariable Variable;

        public VectorReference()
        { }

        public VectorReference(Vector4 value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public int VectorLength
        {
            get { return UseConstant ? 4 : Variable.VectorLength; }
        }

        public bool IsAVectorInt
        {
            get { return UseConstant ? false : Variable.IsAVectorInt; }
        }

        
        public Vector2 ValueVector2
        {
            get { return UseConstant ? (Vector2)(ConstantValue) : Variable.ValueVector2; }
        }

        public Vector3 ValueVector3
        {
            get { return UseConstant ? (Vector3)(ConstantValue) : Variable.ValueVector3; }
        }

        public Vector4 ValueVector4
        {
            get { return UseConstant ? (Vector4)(ConstantValue) : Variable.ValueVector4; }
        }

        public Vector2Int ValueVector2Int
        {
            get { return UseConstant ? ((Vector2)(ConstantValue)).ToVector2Int() : Variable.ValueVector2Int; }
        }

        public Vector3Int ValueVector3Int
        {
            get { return UseConstant ? ((Vector3)(ConstantValue)).ToVector3Int() : Variable.ValueVector3Int; }
        }


        public static implicit operator Vector2(VectorReference reference)
        {
            return reference.ValueVector2;
        }

        public static implicit operator Vector3(VectorReference reference)
        {
            return reference.ValueVector3;
        }

        public static implicit operator Vector4(VectorReference reference)
        {
            return reference.ValueVector4;
        }

        public static implicit operator Vector2Int(VectorReference reference)
        {
            return reference.ValueVector2Int;
        }

        public static implicit operator Vector3Int(VectorReference reference)
        {
            return reference.ValueVector3Int;
        }

    }
}