using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 8)]
    public class Vector4Variable : VectorVariable
    {
        public override int VectorLength
            => 4;
        
        public new Vector4 Value
        {
            get => ValueVector4;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
    }
}
