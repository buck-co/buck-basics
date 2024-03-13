using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : BaseVariable<Vector3Int>, VectorVariable
    {
        public void SetValue(Vector3IntVariable value)
            => Value = value.Value;

        public int VectorLength
            => 3;
        
        public bool IsAVectorInt
            => true;
        
        public Vector2 ValueVector2
            => (Vector2)((Vector3)(Value));

        public Vector3 ValueVector3
            => (Vector3)Value;

        public Vector4 ValueVector4
            => (Vector4)((Vector3)(Value));
        
        public Vector2Int ValueVector2Int
            => (Vector2Int)Value;

        public Vector3Int ValueVector3Int
            => Value;
    }
}