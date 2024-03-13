using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : BaseVariable<Vector2>, VectorVariable
    {
        public void SetValue(Vector2Variable value)
            => Value = value.Value;

        public int VectorLength
            => 2;
        
        public bool IsAVectorInt
            => false;
        
        public Vector2 ValueVector2
            => Value;

        public Vector3 ValueVector3
            => (Vector3)Value;

        public Vector4 ValueVector4
            => (Vector4)Value;
        
        public Vector2Int ValueVector2Int
            => Value.ToVector2Int();

        public Vector3Int ValueVector3Int
            => ((Vector3)Value).ToVector3Int();
    }
}