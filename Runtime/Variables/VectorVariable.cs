using UnityEngine;

namespace Buck
{
    public interface VectorVariable
    {
        public int VectorLength { get; }
        public bool IsAVectorInt { get; }
        public Vector2 ValueVector2 { get; }
        public Vector3 ValueVector3 { get; }
        public Vector4 ValueVector4 { get; }
        public Vector2Int ValueVector2Int { get; }
        public Vector3Int ValueVector3Int { get; }

        public void Raise();
    }
}
