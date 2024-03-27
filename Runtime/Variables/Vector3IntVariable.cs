using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : VectorVariable
    {
        public override int VectorLength
            => 3;
        
        public override bool IsAVectorInt
            => true;
        
        public new Vector3Int Value
        {
            get => ValueVector3Int;
            set
            {
                m_currentValue = (Vector3)value;
                LogValueChange();
            }
        }
    }
}