// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    [CreateAssetMenu(menuName = "BUCK/Variables/Bool Variable", order = 2)] // Default unset order is 1 so list starts at 2
    public class BoolVariable : BaseVariable<bool>
    {
        //TODO: Is there a way to emit a warning if you write if (m_someBoolVariable == true) when you meant (m_someBoolVariable.CurrentValue == true)
        //Try Implicit Operators, try IComparable and IEquatable
    }
}
