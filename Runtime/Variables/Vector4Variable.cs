using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 8)]
    public class Vector4Variable : VectorVariable
    {
        public void SetValue(Vector4Variable value)
            => Value = value.Value;
        
        public override int VectorLength
            => 4;
        
        public override bool IsAVectorInt
            => false;
    }
}
