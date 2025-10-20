// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using TMPro;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// Common base for menu-like views that have a title and CanvasGroup visibility.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MenuView : MonoBehaviour
    {
        [Header("Title")]
        [SerializeField, Tooltip("Title shown in headers/pagers for this view.")]
        UILabel m_title = new UILabel();
        
        [Header("Title Target (optional)")]
        [SerializeField, Tooltip("Optional target to set the title text on when this view is shown, such as a header.")]
        TMP_Text m_titleLabelTMP;

        [Tooltip("Should this view be visible when enabled?")]
        [SerializeField] protected bool m_startVisible = false;

        protected CanvasGroup m_canvasGroup;

        /// <summary>Structured title label for this view.</summary>
        public UILabel Title => m_title;

        /// <summary>Resolved title text for this view.</summary>
        public virtual string TitleText => m_title != null ? m_title.Text : string.Empty;

        protected virtual void Reset()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Awake()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
            
            if (m_titleLabelTMP)
                m_title.BindTo(m_titleLabelTMP);
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
    }
}
