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

        public int VectorLength
        {
            get { return UseVariable ? Variable.VectorLength : 4; }
        }

        public bool IsAVectorInt
        {
            get { return UseVariable ? Variable.IsAVectorInt : false; }
        }

        
        public Vector2 ValueVector2
        {
            get { return UseVariable ? Variable.ValueVector2 : (Vector2)(ConstantValue); }
        }

        public Vector3 ValueVector3
        {
            get { return UseVariable ? Variable.ValueVector3 : (Vector3)(ConstantValue); }
        }

        public Vector4 ValueVector4
        {
            get { return UseVariable ? Variable.ValueVector4 : (Vector4)(ConstantValue); }
        }

        public Vector2Int ValueVector2Int
        {
            get { return UseVariable ? Variable.ValueVector2Int:((Vector2)(ConstantValue)).ToVector2Int(); }
        }

        public Vector3Int ValueVector3Int
        {
            get { return UseVariable ? Variable.ValueVector3Int:((Vector3)(ConstantValue)).ToVector3Int(); }
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