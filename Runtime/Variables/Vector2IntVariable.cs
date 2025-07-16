// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable", order = 9)]
    public class Vector2IntVariable : VectorVariable
    {
        public override int VectorLength
            => 2;
        
        public override bool IsAVectorInt
            => true;

        public new Vector2Int Value
        {
            get => ValueVector2Int;
            set
            {
                m_currentValue = (Vector2)value;
                LogValueChange();
            }
        }
    }
}
