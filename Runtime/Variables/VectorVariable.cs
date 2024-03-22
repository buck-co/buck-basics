using UnityEngine;

namespace Buck
{
    public abstract class VectorVariable : BaseVariable<Vector4>
    {
        public abstract int VectorLength { get; }
        public abstract bool IsAVectorInt { get; }
        
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
