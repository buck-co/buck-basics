using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseVariable
    {
        public GameObject DefaultValue;
        
        private GameObject m_currentValue;
        public GameObject Value
        {
            get { return m_currentValue; }
            set {
                m_currentValue = value;
                LogValueChange((m_currentValue != null)?m_currentValue.name:"null");
            }
        }

        public void SetValue(GameObject value)
        {
            Value = value;
        }

        public void SetValue(GameObjectVariable value)
        {
            Value = value.Value;
        }

        private void OnEnable()
        {
            m_currentValue = DefaultValue;
        }
    }
}