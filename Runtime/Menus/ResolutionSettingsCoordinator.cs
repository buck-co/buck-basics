// MIT License Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    [AddComponentMenu("BUCK/Display/Resolution Settings Coordinator")]
    public class ResolutionSettingsCoordinator : MonoBehaviour
    {
        [Header("Provider")]
        [SerializeField] ResolutionChoiceProvider m_provider;

        [Header("Toggles")]
        [SerializeField] Toggle m_autoResolutionToggle;
        [SerializeField] Toggle m_fullScreenToggle;

        [Header("Dropdown (optional)")]
        [SerializeField] TMP_Dropdown m_resolutionDropdown;
        [SerializeField] bool m_disableDropdownWhenAuto = true;

        // Optional authoritative sources (resolved from VariableBinding if left null)
        [SerializeField] BoolVariable m_autoVariable;
        [SerializeField] BoolVariable m_fullScreenVariable;

        void OnEnable()
        {
            if (!m_provider) m_provider = GetComponent<ResolutionChoiceProvider>();
            TryResolveVariablesFromBindings();

            if (m_autoResolutionToggle)
                m_autoResolutionToggle.onValueChanged.AddListener(OnAutoChanged);

            if (m_fullScreenToggle)
                m_fullScreenToggle.onValueChanged.AddListener(OnFullscreenChanged);

            // Ensure the provider has built its list before we drive it.
            m_provider.Initialize();

            var autoOn = GetAutoOn();
            var fullOn = GetFullOn();

            SyncDropdownInteractable(autoOn);

            // Order: set mode (doesn't change size), then optionally set size if Auto is on.
            m_provider.ApplyFullscreen(fullOn);
            
            // If auto is on, apply it; otherwise reapply current selection or the closest selection.
            if (autoOn)
                m_provider.ApplyAuto();
            else
                m_provider.ReapplyCurrentSelectionOrClosest();
        }

        void OnDisable()
        {
            if (m_autoResolutionToggle)
                m_autoResolutionToggle.onValueChanged.RemoveListener(OnAutoChanged);

            if (m_fullScreenToggle)
                m_fullScreenToggle.onValueChanged.RemoveListener(OnFullscreenChanged);
        }

        void OnAutoChanged(bool on)
        {
            SyncDropdownInteractable(on);
            if (on) m_provider.ApplyAuto();
        }

        void OnFullscreenChanged(bool full)
        {
            m_provider.ApplyFullscreen(full);
        }

        void SyncDropdownInteractable(bool autoOn)
        {
            if (!m_resolutionDropdown || !m_disableDropdownWhenAuto) return;
            m_resolutionDropdown.interactable = !autoOn;
        }

        // Resolve BoolVariables from VariableBinding on the toggles if not explicitly wired.
        void TryResolveVariablesFromBindings()
        {
            if (!m_autoVariable && m_autoResolutionToggle)
            {
                var vb = m_autoResolutionToggle.GetComponent<VariableBinding>();
                if (vb && vb.Variable is BoolVariable bv) m_autoVariable = bv;
            }

            if (!m_fullScreenVariable && m_fullScreenToggle)
            {
                var vb = m_fullScreenToggle.GetComponent<VariableBinding>();
                if (vb && vb.Variable is BoolVariable bv) m_fullScreenVariable = bv;
            }
        }

        bool GetAutoOn()
            => m_autoVariable ? m_autoVariable.Value : (m_autoResolutionToggle && m_autoResolutionToggle.isOn);

        bool GetFullOn()
            => m_fullScreenVariable ? m_fullScreenVariable.Value : (m_fullScreenToggle && m_fullScreenToggle.isOn);
    }
}
