using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3 Variable", order = 7)]
    public class Vector3Variable : VectorVariable
    {
        public Vector3 DefaultValue = Vector3.zero;
        
        private Vector3 m_currentValue;
        public Vector3 Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Vector3 value)
        {
            Value = value;
        }

        public void SetValue(Vector3Variable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }

        public override int VectorLength => 3;
        public override bool IsAVectorInt => false;
        
        public override Vector2 ValueVector2 => (Vector2)(Value);

        public override Vector3 ValueVector3 => Value;

        public override Vector4 ValueVector4 => (Vector4)(Value);
        
        public override Vector2Int ValueVector2Int => (Vector2Int)(Value.ToVector3Int());

        public override Vector3Int ValueVector3Int => Value.ToVector3Int();
    }
}