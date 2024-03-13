using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseVariable<GameObject>
    {
        public override string ValueAsString
            => m_currentValue != null ? m_currentValue.name : "null";

        public void SetValue(GameObjectVariable value)
            => Value = value.Value;
    }
}