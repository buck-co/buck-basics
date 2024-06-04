using UnityEngine;

namespace Buck
{
    public abstract class VectorVariable : BaseVariable<Vector4>
    {
        public abstract int VectorLength { get; }
        
        public virtual bool IsAVectorInt
            => false;
        
        public Vector2 ValueVector2
            => m_currentValue;
        
        public Vector3 ValueVector3
            => m_currentValue;
        
        public Vector4 ValueVector4
            => m_currentValue;
        
        public Vector2Int ValueVector2Int
            => ((Vector2)m_currentValue).ToVector2Int();

        public Vector3Int ValueVector3Int
            => ((Vector3)m_currentValue).ToVector3Int();
    }
}
