using UnityEngine;
using UnityEngine.UI;
using System;

namespace Buck
{
    /// <summary>
    /// Menu item for NumberVariable types (IntVariable, FloatVariable, DoubleVariable), displays as a slider
    /// </summary>
    public class NumberMenuItem : GameMenuItem
    {
        [SerializeField] Slider m_slider;
        
        NumberVariable m_numberVariable;
        GameEventListener m_listener;
        
        protected override void SetupVariableListeners()
        {
            m_numberVariable = m_variable as NumberVariable;
            if (m_numberVariable == null)
            {
                Debug.LogError($"NumberMenuItem requires a NumberVariable, but got {m_variable?.GetType()}");
                return;
            }
            
            if (m_slider == null)
            {
                Debug.LogError($"NumberMenuItem {gameObject.name} is missing a Slider component reference!");
                return;
            }
            
            // Create listener for variable changes
            m_listener = gameObject.AddComponent<GameEventListener>();
            m_listener.Event = m_numberVariable;
            if (m_listener.Response == null)
                m_listener.Response = new UnityEngine.Events.UnityEvent();
            m_listener.Response.AddListener(OnVariableChanged);
            m_listener.enabled = true; // Ensure it's enabled
            
            // Setup slider range from variable's clamp settings
            SetupSliderRange();
            m_slider.onValueChanged.RemoveListener(OnSliderValueChanged);
            m_slider.onValueChanged.AddListener(OnSliderValueChanged);
            
            // Initialize display with current value
            UpdateDisplay();
        }
        
        void SetupSliderRange()
        {
            float min = 0f;
            float max = 1f;
            bool hasValidRange = false;
            
            if (m_numberVariable != null)
            {
                // Check if clamping is enabled
                if (m_numberVariable.ClampToAMin && m_numberVariable.ClampMin != null)
                {
                    min = (float)m_numberVariable.ClampMin;
                    hasValidRange = true;
                }
                
                if (m_numberVariable.ClampToAMax && m_numberVariable.ClampMax != null)
                {
                    max = (float)m_numberVariable.ClampMax;
                    hasValidRange = true;
                }
                
                // If no clamping is set, emit warning and use default
                if (!m_numberVariable.ClampToAMin && !m_numberVariable.ClampToAMax)
                {
                    Debug.LogWarning($"NumberVariable '{m_numberVariable.name}' has no clamp range set. Using default range 0-1 for menu slider.");
                }
                else if (!m_numberVariable.ClampToAMin)
                {
                    Debug.LogWarning($"NumberVariable '{m_numberVariable.name}' has no minimum clamp set. Using 0 as minimum for menu slider.");
                }
                else if (!m_numberVariable.ClampToAMax)
                {
                    Debug.LogWarning($"NumberVariable '{m_numberVariable.name}' has no maximum clamp set. Using 1 as maximum for menu slider.");
                }
            }
            
            m_slider.minValue = min;
            m_slider.maxValue = max;
        }
        
        protected override void RemoveVariableListeners()
        {
            if (m_listener != null)
            {
                m_listener.Response.RemoveListener(OnVariableChanged);
                Destroy(m_listener);
            }
            
            if (m_slider != null)
                m_slider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
        
        protected override void UpdateDisplay()
        {
            if (m_numberVariable == null) return;
            
            double value = m_numberVariable.Value;
            float floatValue = (float)value;
            
            if (m_slider != null)
            {
                m_slider.SetValueWithoutNotify(floatValue);
            }
        }
        
        void OnVariableChanged()
        {
            UpdateDisplay();
        }
        
        void OnSliderValueChanged(float value)
        {
            if (m_numberVariable == null) return;
            
            m_numberVariable.Value = value;
            m_numberVariable.Raise();
        }
        
        public override void HandleHorizontalInput(float input)
        {
            if (m_numberVariable != null && m_slider != null && Mathf.Abs(input) > 0.5f)
            {
                // Move slider by a percentage of the range
                float range = m_slider.maxValue - m_slider.minValue;
                float delta = range * 0.01f * Mathf.Sign(input); // 1% of range per input
                
                float currentValue = m_slider.value;
                float newValue = Mathf.Clamp(currentValue + delta, m_slider.minValue, m_slider.maxValue);
                
                m_slider.value = newValue;
            }
        }
    }
}