using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 8)]
    public class Vector4Variable : BaseVariable<Vector4>, VectorVariable
    {
        public void SetValue(Vector4Variable value)
            => Value = value.Value;
        
        public int VectorLength
            => 4;
        
        public bool IsAVectorInt
            => false;
        
        public Vector2 ValueVector2
            => (Vector2)Value;

        public Vector3 ValueVector3
            => (Vector3)Value;

        public Vector4 ValueVector4
            => Value;
        
        public Vector2Int ValueVector2Int
            => ((Vector2)Value).ToVector2Int();

        public Vector3Int ValueVector3Int
            => ((Vector3)Value).ToVector3Int();
    }
}
