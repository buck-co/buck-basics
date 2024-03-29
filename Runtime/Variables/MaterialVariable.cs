using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Material Variable", order = 17)]
    public class MaterialVariable : BaseVariable<Material>
    {
        public override string ToString()
            => Value != null ? Value.name : "null";
    }
}