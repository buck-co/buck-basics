using UnityEngine;

namespace Buck
{
    public abstract class VectorVariable : BaseVariable
    {
        public abstract int VectorLength { get; }
        public abstract bool IsAVectorInt { get; }
        public abstract Vector2 ValueVector2 { get; }
        public abstract Vector3 ValueVector3 { get; }
        public abstract Vector4 ValueVector4 { get; }
        public abstract Vector2Int ValueVector2Int { get; }
        public abstract Vector3Int ValueVector3Int { get; }
    }
}
