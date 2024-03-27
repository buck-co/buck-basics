using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseVariable<GameObject>
    {
        public override string ToString()
            => m_currentValue != null ? m_currentValue.name : "null";
    }
}