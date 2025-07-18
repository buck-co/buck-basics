﻿// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using UnityEngine;

namespace Buck
{
    [Serializable]
    public class ColorReference
    {
        public bool UseVariable = false;
        public Color ConstantValue = Color.white;
        public ColorVariable Variable;

        public ColorReference()
        { }

        public ColorReference(Color value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public ColorReference(ColorVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public Color Value
            => UseVariable ? Variable.Value : ConstantValue;

        public static implicit operator Color(ColorReference reference)
            => reference.Value;
    }
}