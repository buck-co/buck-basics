using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable", order = 7)]
    public class Vector3Variable : VectorVariable
    {
        public void SetValue(Vector3Variable value)
            => Value = value.Value;

        public override int VectorLength
            => 3;
        
        public override bool IsAVectorInt
            => false;
    }
}