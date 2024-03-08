using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Sprite Variable", order = 16)]
    public class SpriteVariable : BaseVariable
    {
        public Sprite DefaultValue;
        
        Sprite m_currentValue;
        public Sprite Value
        {
            get => m_currentValue;
            set
            { 
                m_currentValue = value;
                LogValueChange();
            }
        }
        
        public override string ValueAsString
            => (m_currentValue != null) ? m_currentValue.name : "null";

        public void SetValue(Sprite value)
            => Value = value;

        public void SetValue(SpriteVariable value)
            => Value = value.Value;

        void OnEnable()
            => m_currentValue = DefaultValue;
    }
}