// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleHelper : UIValueBinder<bool, BoolVariable, Toggle>
    {
        protected override void SubscribeControlEvents()
            => m_control.onValueChanged.AddListener(OnControlValueChanged);

        protected override void UnsubscribeControlEvents()
            => m_control.onValueChanged.RemoveListener(OnControlValueChanged);

        protected override void SetControlValueWithoutNotify(bool value)
            => m_control.SetIsOnWithoutNotify(value);

        public void SetVariable(BoolVariable variable, bool raiseEvent = true)
        {
            m_variable = variable;
            m_raiseGameEventOnChange = raiseEvent;
        }
        
        protected override bool GetVariableValue() => m_variable.Value;

        protected override void SetVariableValue(bool value)
        {
            m_variable.Value = value;
            if (m_raiseGameEventOnChange)
                m_variable.Raise();
        }

        protected override string GetVariableLabel() => m_variable.LabelText ?? string.Empty;
    }
}
