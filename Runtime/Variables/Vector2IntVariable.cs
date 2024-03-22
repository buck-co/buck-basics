using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable", order = 9)]
    public class Vector2IntVariable : VectorVariable
    {
        public void SetValue(Vector2IntVariable value)
            => Value = value.Value;

        public override int VectorLength
            => 2;
        
        public override bool IsAVectorInt
            => true;
    }
}
