// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : VectorVariable
    {
        public override int VectorLength
            => 2;
        
        public new Vector2 Value
        {
            get => ValueVector2;
            set
            {
                m_currentValue = value;
                LogValueChange();
            }
        }
    }
}