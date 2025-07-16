// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Texture2D Variable", order = 15)]
    public class Texture2DVariable : BaseVariable<Texture2D>
    {
        public override string ToString()
            => Value != null ? Value.name : name + ".Value is null.";
    }
}