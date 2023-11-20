using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector4 Variable", order = 8)]
    public class Vector4Variable : VectorVariable
    {
        public Vector4 DefaultValue = Vector4.zero;
        
        private Vector4 m_currentValue;
        public Vector4 Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Vector4 value)
        {
            Value = value;
        }

        public void SetValue(Vector4Variable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
        

        public override int VectorLength => 4;
        public override bool IsAVectorInt => false;
        
        public override Vector2 ValueVector2 => (Vector2)(Value);

        public override Vector3 ValueVector3 => (Vector3)(Value);

        public override Vector4 ValueVector4 => Value;
        
        public override Vector2Int ValueVector2Int => ((Vector2)Value).ToVector2Int();

        public override Vector3Int ValueVector3Int => ((Vector3)Value).ToVector3Int();
    }
}