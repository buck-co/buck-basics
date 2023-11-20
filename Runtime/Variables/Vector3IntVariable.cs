using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector3Int Variable", order = 10)]
    public class Vector3IntVariable : VectorVariable
    {
        public Vector3Int DefaultValue = Vector3Int.zero;
        
        private Vector3Int m_currentValue;
        public Vector3Int Value
        {
            get { return m_currentValue; }
            set { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        public override string ValueAsString => m_currentValue.ToString();

        public void SetValue(Vector3Int value)
        {
            Value = value;
        }

        public void SetValue(Vector3IntVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
        

        public override int VectorLength => 3;
        public override bool IsAVectorInt => true;
        
        public override Vector2 ValueVector2 => (Vector2)((Vector3)(Value));

        public override Vector3 ValueVector3 => (Vector3)(Value);

        public override Vector4 ValueVector4 => (Vector4)((Vector3)(Value));
        
        public override Vector2Int ValueVector2Int => (Vector2Int)(Value);

        public override Vector3Int ValueVector3Int => Value;
    }
}