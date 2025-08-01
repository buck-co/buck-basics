// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if USE_LOCALIZATION
using UnityEngine.Localization;
#endif

using System;

namespace Buck
{
    [Serializable]
    public class StringReference
    {
        public bool UseLocalizedString = false;
        
        public bool UseVariable = false;
        public string ConstantValue;
        public StringVariable Variable;
        
#if USE_LOCALIZATION
        public LocalizedString LocalizedString;
#endif

        public StringReference()
        { }

        public StringReference(string value)
        {
            UseVariable = false;
            ConstantValue = value;
        }
        
        public StringReference(StringVariable value)
        {
            UseVariable = true;
            Variable = value;
        }

        public string Value
#if USE_LOCALIZATION
            => UseLocalizedString ? LocalizedString.GetLocalizedString() : UseVariable ? Variable.Value : ConstantValue;
#else
            => UseVariable ? Variable.Value : ConstantValue;
#endif

        public static implicit operator string(StringReference reference)
            => reference.Value;
    }
}