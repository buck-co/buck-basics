// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Buck
{
    /// <summary>
    /// Menu item for BoolVariable, displays as a toggle/checkbox
    /// </summary>
    public class BoolMenuItem : GameMenuItem
    {
        [SerializeField] Toggle m_toggle;
        
        BoolVariable m_boolVariable;
        GameEventListener m_listener;
        
        protected override void SetupVariableListeners()
        {
            m_boolVariable = m_variable as BoolVariable;
            if (m_boolVariable == null)
            {
                Debug.LogError($"BoolMenuItem requires a BoolVariable, but got {m_variable?.GetType()}");
                return;
            }
            
            if (m_toggle == null)
            {
                Debug.LogError($"BoolMenuItem {gameObject.name} is missing a Toggle component reference!");
                return;
            }
            
            // Create listener for variable changes
            m_listener = gameObject.AddComponent<GameEventListener>();
            m_listener.Event = m_boolVariable;
            if (m_listener.Response == null)
                m_listener.Response = new UnityEngine.Events.UnityEvent();
            m_listener.Response.AddListener(OnVariableChanged);
            m_listener.enabled = true; // Ensure it's enabled
            
            // Setup toggle listener
            m_toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            m_toggle.onValueChanged.AddListener(OnToggleValueChanged);
            
            // Initialize display with current value
            UpdateDisplay();
        }
        
        protected override void RemoveVariableListeners()
        {
            if (m_listener != null)
            {
                m_listener.Response.RemoveListener(OnVariableChanged);
                Destroy(m_listener);
            }
            
            if (m_toggle != null)
                m_toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
        
        protected override void UpdateDisplay()
        {
            if (m_boolVariable == null) return;
            
            bool value = m_boolVariable.Value;
            
            if (m_toggle != null)
            {
                m_toggle.SetIsOnWithoutNotify(value);
            }
        }
        
        void OnVariableChanged()
        {
            UpdateDisplay();
        }
        
        void OnToggleValueChanged(bool value)
        {
            if (m_boolVariable != null)
            {
                m_boolVariable.Value = value;
                m_boolVariable.Raise();
            }
        }
        
        public override void HandleHorizontalInput(float input)
        {
            if (m_boolVariable != null && Mathf.Abs(input) > 0.5f)
            {
                m_boolVariable.Value = !m_boolVariable.Value;
                m_boolVariable.Raise();
                UpdateDisplay();
            }
        }
    }
}