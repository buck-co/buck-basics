// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Buck
{
    /// <summary>
    /// Simple pager UI that shows the current page title and exposes Next/Prev methods.
    /// Should be a child of the same MenuController as the pages.
    /// Contains no Selectables, so that focus isn't stolen from the current page.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Pager (Bumper Bar)")]
    public class MenuPager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] MenuController m_controller;
        [SerializeField] TextMeshProUGUI m_titleText;

        [Header("Events (optional)")]
        [SerializeField] UnityEvent<string> m_onTitleChanged;

#region Unity Lifecycle

        void Awake()
        {
            if (!m_controller)
                m_controller = MenuController.FindFor(transform);
        }

        void OnEnable()
        {
            if (m_controller)
            {
                m_controller.OnOpenMenu          += HandleScreenChanged; // fire when first menu opens and on pushes
                m_controller.OnOpenSiblingMenu   += HandleScreenChanged; // fire when flipping pages
                m_controller.OnBack              += HandleScreenChanged; // update when backing
                m_controller.OnStackEmptyChanged += HandleStackEmptyChanged;
            }
            Refresh();
        }

        void OnDisable()
        {
            if (!m_controller) return;
            
            m_controller.OnOpenMenu          -= HandleScreenChanged;
            m_controller.OnOpenSiblingMenu   -= HandleScreenChanged;
            m_controller.OnBack              -= HandleScreenChanged;
            m_controller.OnStackEmptyChanged -= HandleStackEmptyChanged;
        }

#endregion

#region Helper Methods

        void HandleScreenChanged(MenuScreen _)
            => Refresh();
        
        void HandleStackEmptyChanged(bool isEmpty)
            => gameObject.SetActive(!isEmpty && ShouldShowFor(m_controller?.Current));

        bool ShouldShowFor(MenuScreen screen)
        {
            if (!screen) return false;
            var group = MenuSiblingGroup.FindFor(screen);
            return group && group.Pages.Count > 1;
        }

        void Refresh()
        {
            var cur = m_controller ? m_controller.Current : null; // top of stack.
            bool show = ShouldShowFor(cur);

            gameObject.SetActive(show);

            if (!show)
                return;

            var title = cur?.TitleText;
            
            if (m_titleText)
                m_titleText.text = string.IsNullOrEmpty(title) ? (cur ? cur.name : "") : title;
            
            m_onTitleChanged?.Invoke(title ?? string.Empty);
        }

#endregion
        
#region Public Methods
        
        public void NextPage()
        {
            var cur = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var next = group?.Next(cur);
            
            if (next)
                m_controller.MenuNav_OpenSiblingMenu(next); // replaces top, keeps parent.
        }

        public void PrevPage()
        {
            var cur = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var prev = group?.Prev(cur);
            
            if (prev)
                m_controller.MenuNav_OpenSiblingMenu(prev);
        }

#endregion
        
    }
}
