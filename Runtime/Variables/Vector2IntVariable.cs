using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Vector2Int Variable", order = 9)]
    public class Vector2IntVariable : BaseVariable
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
    }
}