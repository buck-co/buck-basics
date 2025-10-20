// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
#if BUCK_BASICS_ENABLE_LOCALIZATION
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
#endif

namespace Buck
{
    /// <summary>
    /// Serializable UI label that can display either plain text or a LocalizedString
    /// and knows how to bind itself to a TextMeshPro label.
    /// </summary>
    [Serializable]
    public class UILabel
    {
        [SerializeField, Tooltip("Literal text used when localization is disabled or not configured.")]
        string m_text = string.Empty;

#if BUCK_BASICS_ENABLE_LOCALIZATION
        [SerializeField, Tooltip("When enabled, uses the Localized Text reference instead of the Text string.")]
        bool m_useLocalizedText = false;

        [SerializeField]
        LocalizedString m_localizedText;
#endif

        /// <summary>
        /// Current resolved text for this label (synchronously resolves when localized).
        /// </summary>
        public string Text
        {
            get
            {
#if BUCK_BASICS_ENABLE_LOCALIZATION
                if (m_useLocalizedText && m_localizedText != null)
                    return m_localizedText.GetLocalizedString();
#endif
                return m_text ?? string.Empty;
            }
        }

        /// <summary>
        /// Bind this label to a TextMeshPro target. When localization is enabled and configured,
        /// a LocalizeStringEvent is added to auto-refresh on locale changes; otherwise the literal text is applied.
        /// Optional callback is invoked whenever text is updated (initially and on locale changes).
        /// </summary>
        public void BindTo(TMP_Text target, UnityAction<string> onTextChanged = null)
        {
            if (!target) return;

#if BUCK_BASICS_ENABLE_LOCALIZATION
            if (m_useLocalizedText && m_localizedText != null)
            {
                var localizeStringEvent = target.GetComponent<LocalizeStringEvent>() ?? target.gameObject.AddComponent<LocalizeStringEvent>();
                localizeStringEvent.StringReference = m_localizedText;
                localizeStringEvent.OnUpdateString.RemoveAllListeners();
                localizeStringEvent.OnUpdateString.AddListener(target.SetText);
                if (onTextChanged != null) localizeStringEvent.OnUpdateString.AddListener(onTextChanged);
                localizeStringEvent.RefreshString();
                return;
            }
            else
            {
                var existing = target.GetComponent<LocalizeStringEvent>();
                if (existing) UnityEngine.Object.Destroy(existing);
            }
#endif
            var current = Text;
            target.text = current;
            onTextChanged?.Invoke(current);
        }

        /// <summary>
        /// Convenience overload that finds a TextMeshPro label under the transform and binds to it.
        /// </summary>
        public void BindUnder(Transform root, UnityAction<string> onTextChanged = null)
        {
            if (!root) return;
            var tmp = root.GetComponentInChildren<TMP_Text>();
            if (tmp) BindTo(tmp, onTextChanged);
        }
    }
}
