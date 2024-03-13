using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable", order = 9)]
    public class Vector2IntVariable : BaseVariable<Vector2Int>, VectorVariable
    {
        public void SetValue(Vector2IntVariable value)
            => Value = value.Value;

        public int VectorLength
            => 2;
        
        public bool IsAVectorInt
            => true;
        
        public Vector2 ValueVector2
            => (Vector2)Value;

        public Vector3 ValueVector3
            => (Vector3)((Vector2)(Value));

        public Vector4 ValueVector4
            => (Vector4)((Vector2)(Value));
        
        public Vector2Int ValueVector2Int
            => Value;

        public Vector3Int ValueVector3Int
            => (Vector3Int)(Value);
    }
}
