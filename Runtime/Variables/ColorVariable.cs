using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Color Variable", order = 14)]
    public class ColorVariable : BaseVariable<Color>
    {
        public void SetValue(ColorVariable value)
            => Value = value.Value;
    }
}