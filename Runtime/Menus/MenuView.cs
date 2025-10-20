// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
#if BUCK_BASICS_ENABLE_LOCALIZATION
using UnityEngine.Localization;
#endif

namespace Buck
{
    /// <summary>
    /// Common base for menu-like views that have a title and CanvasGroup visibility.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MenuView : MonoBehaviour
    {
        [Header("Metadata")]
        [SerializeField, Tooltip("Title shown in headers/pagers for this view.")]
        string m_titleText = "";

#if BUCK_BASICS_ENABLE_LOCALIZATION
        [SerializeField, Tooltip("When enabled, TitleText uses the Localized Title Text instead of the Title Text string.")]
        bool m_localizeTitleText = false;
        [SerializeField] LocalizedString m_localizedTitleText;
#endif
        
        [Header("Title Target (optional)")]
        [SerializeField, Tooltip("Optional target to set the title text on when this view is shown, such as a header.")]
        TMP_Text m_titleLabelTMP;
#if UNITY_UGUI_PRESENT
        [SerializeField, Tooltip("Optional target to set the title text on when this view is shown, such as a header.")]
        UnityEngine.UI.Text m_titleLabelUGUI;
#endif

        /// <summary>
        /// The title text to display for this view (supports localization if enabled).
        /// </summary>
        public virtual string TitleText
        {
            get
            {
#if BUCK_BASICS_ENABLE_LOCALIZATION
                if (m_localizeTitleText && m_localizedTitleText != null)
                    return m_localizedTitleText.GetLocalizedString();
#endif
                return m_titleText;
            }
        }

        [Tooltip("Should this view be visible when enabled?")]
        [SerializeField] protected bool m_startVisible = false;

        protected CanvasGroup m_canvasGroup;

        protected virtual void Reset()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Awake()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
            
            BindTitleIfPossible(); // bind once; LocalizeStringEvent will keep it up to date
        }

        protected virtual void OnEnable()
            => Toggle(m_startVisible);

        /// <summary>Show this view.</summary>
        public virtual void Show(bool focusFirst = false)
        {
            if (m_canvasGroup) m_canvasGroup.SetVisible(true);
        }

        /// <summary>Hide this view.</summary>
        public virtual void Hide()
        {
            if (m_canvasGroup) m_canvasGroup.SetVisible(false);
        }

        /// <summary>Set visibility.</summary>
        public void Toggle(bool visible)
        {
            if (visible)
                Show();
            else
                Hide();
        }

        /// <summary>True if this view is interactable and visible.</summary>
        protected bool IsVisible()
            => m_canvasGroup.IsVisible();
        
#if BUCK_BASICS_ENABLE_LOCALIZATION
        public bool TryGetTitleLocalizedString(out LocalizedString localized)
        {
            if (m_localizeTitleText && m_localizedTitleText != null)
            {
                localized = m_localizedTitleText;
                return true;
            }
            localized = null;
            return false;
        }
#endif
        
        void BindTitleIfPossible()
        {
            // Prefer TextMeshPro
            if (m_titleLabelTMP)
            {
#if BUCK_BASICS_ENABLE_LOCALIZATION
                if (m_localizeTitleText && m_localizedTitleText != null)
                {
                    var localizeStringEvent = m_titleLabelTMP.GetComponent<LocalizeStringEvent>() ?? m_titleLabelTMP.gameObject.AddComponent<LocalizeStringEvent>();
                    localizeStringEvent.StringReference = m_localizedTitleText;
                    localizeStringEvent.OnUpdateString.RemoveAllListeners();
                    localizeStringEvent.OnUpdateString.AddListener(m_titleLabelTMP.SetText);
                    localizeStringEvent.RefreshString();
                    return;
                }
#endif
                m_titleLabelTMP.text = TitleText;
                return;
            }

#if UNITY_UGUI_PRESENT
            if (m_titleLabelUGUI)
            {
#if BUCK_BASICS_ENABLE_LOCALIZATION
                if (m_localizeTitleText && m_localizedTitleText != null)
                {
                    var localizeStringEvent = m_titleLabelUGUI.GetComponent<LocalizeStringEvent>() ?? m_titleLabelUGUI.gameObject.AddComponent<LocalizeStringEvent>();
                    localizeStringEvent.StringReference = m_localizedTitleText;
                    localizeStringEvent.OnUpdateString.RemoveAllListeners();
                    localizeStringEvent.OnUpdateString.AddListener(s => m_titleLabelUGUI.text = s);
                    localizeStringEvent.RefreshString();
                    return;
                }
#endif
                m_titleLabelUGUI.text = TitleText;
            }
#endif
        }
    }
}
