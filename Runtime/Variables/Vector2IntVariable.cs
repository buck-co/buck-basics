using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable", order = 9)]
    public class Vector2IntVariable : VectorVariable
    {
        public Vector2Int DefaultValue = Vector2Int.zero;
        
        private Vector2Int m_currentValue;
        public Vector2Int Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
                }
        }
        public override string ValueAsString => m_currentValue.ToString();


        public void SetValue(Vector2Int value)
        {
            Value = value;
        }

        public void SetValue(Vector2IntVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }

        public override int VectorLength => 2;
        public override bool IsAVectorInt => true;
        
        public override Vector2 ValueVector2 => (Vector2)(Value);

        public override Vector3 ValueVector3 => (Vector3)((Vector2)(Value));

        public override Vector4 ValueVector4 => (Vector4)((Vector2)(Value));
        
        public override Vector2Int ValueVector2Int => Value;

        public override Vector3Int ValueVector3Int => (Vector3Int)(Value);
    }
}