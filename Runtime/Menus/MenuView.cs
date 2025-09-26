// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
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
        }

        protected virtual void OnEnable()
        {
            if (m_startVisible)
                Show();
            else
                Hide();
        }

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
            if (visible) Show();
            else Hide();
        }

        /// <summary>True if this view is interactable and visible.</summary>
        protected bool IsVisible()
            => m_canvasGroup && m_canvasGroup.interactable && m_canvasGroup.blocksRaycasts && m_canvasGroup.alpha > 0.0f;
    }
}
