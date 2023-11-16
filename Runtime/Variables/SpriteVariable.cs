using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Sprite Variable", order = 16)]
    public class SpriteVariable : BaseScriptableObject
    {
        public Sprite DefaultValue;
        
        private Sprite currentValue;
        public Sprite CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;}
        }

        public void SetValue(Sprite value)
        {
            CurrentValue = value;
        }

        public void SetValue(SpriteVariable value)
        {
            CurrentValue = value.CurrentValue;
        }

        private void OnEnable()
        {
            currentValue = DefaultValue;
        }
    }
}