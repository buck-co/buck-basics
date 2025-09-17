// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    [RequireComponent(typeof(Slider))]
    public class UISliderHelper : UIValueBinder<float, FloatVariable, Slider>
    {
        protected override void SubscribeControlEvents()
            => m_control.onValueChanged.AddListener(OnControlValueChanged);

        protected override void UnsubscribeControlEvents()
            => m_control.onValueChanged.RemoveListener(OnControlValueChanged);

        protected override void SetControlValueWithoutNotify(float value)
            => m_control.SetValueWithoutNotify(value);

        protected override float GetVariableValue() => m_variable.Value;

        public void SetVariable(FloatVariable variable, bool raiseEvent = true)
        {
            m_variable = variable;
            m_raiseGameEventOnChange = raiseEvent;
        }

        protected override void SetVariableValue(float value)
        {
            m_variable.Value = value;
            if (m_raiseGameEventOnChange)
                m_variable.Raise();
        }

        protected override string GetVariableLabel() => m_variable.LabelText ?? string.Empty;
    }
}
