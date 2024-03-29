using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseVariable<GameObject>
    {
        public override string ToString()
            => Value != null ? Value.name : "null";
    }
}