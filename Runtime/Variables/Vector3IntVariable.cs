using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : VectorVariable
    {
        public void SetValue(Vector3IntVariable value)
            => Value = value.Value;

        public override int VectorLength
            => 3;
        
        public override bool IsAVectorInt
            => true;
    }
}