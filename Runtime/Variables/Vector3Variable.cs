using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable", order = 7)]
    public class Vector3Variable : BaseVariable<Vector3>, VectorVariable
    {
        public void SetValue(Vector3Variable value)
            => Value = value.Value;

        public int VectorLength
            => 3;
        
        public bool IsAVectorInt
            => false;
        
        public Vector2 ValueVector2
            => (Vector2)Value;

        public Vector3 ValueVector3
            => Value;

        public Vector4 ValueVector4
            => (Vector4)Value;
        
        public Vector2Int ValueVector2Int
            => (Vector2Int)Value.ToVector3Int();

        public Vector3Int ValueVector3Int
            => Value.ToVector3Int();
    }
}