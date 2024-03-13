using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/String Variable", order = 12)]
    public class StringVariable : BaseVariable<string>
    {
        public void SetValue(StringVariable value)
            => Value = value.Value;
    }
}
