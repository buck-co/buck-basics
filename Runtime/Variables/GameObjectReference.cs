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

        public GameObject Value
        {
            get { return UseVariable ? Variable.Value : ConstantValue; }
        }

        public static implicit operator GameObject(GameObjectReference reference)
        {
            return reference.Value;
        }
    }
}