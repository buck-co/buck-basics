using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Sprite Variable", order = 16)]
    public class SpriteVariable : BaseVariable<Sprite>
    {
        public override string ToString()
            => m_currentValue != null ? m_currentValue.name : "null";
    }
}