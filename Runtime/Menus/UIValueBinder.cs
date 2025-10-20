// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace Buck
{
    /// <summary>
    /// Non-generic surface so screens can refresh all binders without reflection.
    /// </summary>
    public interface IUIValueBinder
    {
        void Initialize();
        void RefreshFromVariable();
    }

    /// <summary>
    /// Generic binder that syncs a ScriptableObject-like variable with a uGUI control.
    /// Derived classes implement how to read/write the variable and wire control events.
    /// </summary>
    public abstract class UIValueBinder<T, TVariable, TControl> : MonoBehaviour, IUIValueBinder
        where TControl : Component
    {
        // The variable bound to this control.
        protected TVariable m_variable;
        
        protected bool m_raiseGameEventOnChange = true;

        protected TControl m_control;
        bool m_initialized;

        /// <summary>
        /// Call manually if you add at runtime; otherwise handled by OnEnable when the variable is already assigned.
        /// </summary>
        public void Initialize()
        {
            if (m_initialized) return;

            m_control = GetComponent<TControl>();
            if (!m_control)
            {
                Debug.LogError($"{GetType().Name}.Initialize() - Missing {typeof(TControl).Name}.", this);
                return;
            }

            if (m_variable == null)
            {
                Debug.LogError($"{GetType().Name}.Initialize() - Variable is not assigned.", this);
                return;
            }

            // Seed UI from variable without notifying control listeners.
            SetControlValueWithoutNotify(GetVariableValue());

            SubscribeControlEvents();
            TrySetLabelText();

            m_initialized = true;
        }

        /// <summary>
        /// Auto-initialize only when the variable is pre-assigned (e.g., via Inspector).
        /// When added at runtime, UIBinderUtility will call SetVariable(...) then Initialize().
        /// </summary>
        protected virtual void OnEnable()
        {
            if (!m_initialized && m_variable != null)
                Initialize();
        }

        protected virtual void OnDisable() => Cleanup();
        protected virtual void OnDestroy() => Cleanup();

        void Cleanup()
        {
            if (!m_initialized) return;
            UnsubscribeControlEvents();
            m_initialized = false;
        }

        /// <summary>
        /// Attempts to set or bind a child text component to the variable's label.
        /// </summary>
        protected void TrySetLabelText()
        {
#if BUCK_BASICS_ENABLE_LOCALIZATION
            // If the variable exposes a LocalizedString, bind via a LocalizeStringEvent component which will auto-refresh on locale changes.
            if (m_variable is BaseVariable baseVar &&
                baseVar.TryGetLabelLocalizedString(out var localized) &&
                localized != null)
            {
                // TMP first
                var tmp = GetComponentInChildren<TMP_Text>();
                if (tmp)
                {
                    var localizeStringEvent = tmp.GetComponent<LocalizeStringEvent>() ?? tmp.gameObject.AddComponent<LocalizeStringEvent>();
                    localizeStringEvent.StringReference = localized;
                    localizeStringEvent.OnUpdateString.RemoveAllListeners();
                    localizeStringEvent.OnUpdateString.AddListener(tmp.SetText);
                    localizeStringEvent.RefreshString(); // immediate update
                    return;
                }
#if UNITY_UGUI_PRESENT
                // Legacy uGUI Text
                var legacy = GetComponentInChildren<UnityEngine.UI.Text>();
                if (legacy)
                {
                    var localizeStringEvent = legacy.GetComponent<LocalizeStringEvent>() ?? legacy.gameObject.AddComponent<LocalizeStringEvent>();
                    localizeStringEvent.StringReference = localized;
                    localizeStringEvent.OnUpdateString.RemoveAllListeners();
                    localizeStringEvent.OnUpdateString.AddListener(s => legacy.text = s);
                    localizeStringEvent.RefreshString();
                    return;
                }
#endif
            }
#endif
            // Fallback: just set the current literal label once.
            var label = GetVariableLabel();
            if (string.IsNullOrEmpty(label)) return;

            var tmp2 = GetComponentInChildren<TMP_Text>();
            if (tmp2) { tmp2.text = label; return; }

#if UNITY_UGUI_PRESENT
            var legacy2 = GetComponentInChildren<UnityEngine.UI.Text>();
            if (legacy2) legacy2.text = label;
#endif
        }

        /// <summary>
        /// One-way pull: update the UI from the variable.
        /// </summary>
        public void RefreshFromVariable()
        {
            if (m_variable == null || m_control == null) return;
            SetControlValueWithoutNotify(GetVariableValue());
        }

        // Abstract hooks for derived concrete classes to implement.
        protected abstract T GetVariableValue();
        protected abstract void SetVariableValue(T value);
        protected abstract string GetVariableLabel();
        protected abstract void SetControlValueWithoutNotify(T value);
        protected abstract void SubscribeControlEvents();
        protected abstract void UnsubscribeControlEvents();

        /// <summary>
        /// Final handler invoked on control change. Default writes into the variable.
        /// </summary>
        protected virtual void OnControlValueChanged(T value) => SetVariableValue(value);
    }
}
