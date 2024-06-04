using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable", order = 7)]
    public class Vector3Variable : VectorVariable
    {
        public override int VectorLength
            => 3;
        
        public new Vector3 Value
        {
            get => ValueVector3;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
    }
}