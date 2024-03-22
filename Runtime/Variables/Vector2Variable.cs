using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : VectorVariable
    {
        public void SetValue(Vector2Variable value)
            => Value = value.Value;

        public override int VectorLength
            => 2;
        
        public override bool IsAVectorInt
            => false;
    }
}