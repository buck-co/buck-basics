using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Texture2D Variable", order = 15)]
    public class Texture2DVariable : BaseVariable<Texture2D>
    {
        public override string ToString()
            => m_currentValue != null ? m_currentValue.name : "null";
    }
}