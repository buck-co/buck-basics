﻿// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/GameObject Variable", order = 13)]
    public class GameObjectVariable : BaseVariable<GameObject>
    {
        public override string ToString()
            => Value != null ? Value.name : name + ".Value is null.";
    }
}