using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Material Variable", order = 17)]
    public class MaterialVariable : BaseVariable<Material>
    {
        public override string ValueAsString
            => m_currentValue != null ? m_currentValue.name : "null";

        public void SetValue(MaterialVariable value)
            => Value = value.Value;
    }
}