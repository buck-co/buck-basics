// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class GameObjectReference
    {
        public bool UseVariable = false;
        public GameObject ConstantValue;
        public GameObjectVariable Variable;

        public GameObjectReference()
        { }

        public GameObjectReference(GameObject value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public GameObjectReference(GameObjectVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public GameObject Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator GameObject(GameObjectReference reference)
            => reference.Value;
    }
}