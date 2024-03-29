using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Sprite Variable", order = 16)]
    public class SpriteVariable : BaseVariable<Sprite>
    {
        public override string ToString()
            => Value != null ? Value.name : "null";
    }
}