using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Quaternion Variable", order = 11)]
    public class QuaternionVariable : BaseVariable<Quaternion>
    {
        public void SetValue(QuaternionVariable value)
            => Value = value.Value;
    }
}
