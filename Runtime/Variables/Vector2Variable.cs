using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2 Variable", order = 6)]
    public class Vector2Variable : VectorVariable
    {
        public Vector2 DefaultValue = Vector2.zero;
        
        Vector2 m_currentValue;
        public Vector2 Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ValueAsString
            => m_currentValue.ToString();

        public void SetValue(Vector2 value)
            => Value = value;

        public void SetValue(Vector2Variable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;

        public override int VectorLength
            => 2;
        
        public override bool IsAVectorInt
            => false;
        
        public override Vector2 ValueVector2
            => Value;

        public override Vector3 ValueVector3
            => (Vector3)(Value);

        public override Vector4 ValueVector4
            => (Vector4)(Value);
        
        public override Vector2Int ValueVector2Int
            => Value.ToVector2Int();

        public override Vector3Int ValueVector3Int
            => ((Vector3)Value).ToVector3Int();
    }
}