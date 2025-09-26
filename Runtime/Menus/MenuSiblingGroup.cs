// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// Declares an ordered set of MenuScreens that are siblings/pages of one conceptual menu.
    /// Manages its own visibility via CanvasGroup based on the current page in the bound MenuController.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Sibling Group")]
    public class MenuSiblingGroup : MenuView
    {
        [Tooltip("If true, paging past the last goes to the first (and vice versa).")]
        [SerializeField] bool m_wrap = true;

        [Tooltip("Ordered list of sibling pages. The first entry is the default first page.")]
        [SerializeField] List<MenuScreen> m_pages = new();

        [Header("Wiring (optional)")]
        [SerializeField] MenuController m_controller;

        static readonly Dictionary<MenuScreen, MenuSiblingGroup> s_lookup = new();

        public IReadOnlyList<MenuScreen> Pages => m_pages;
        public bool Wrap => m_wrap;

        protected override void Awake()
        {
            base.Awake();
            if (!m_controller)
                m_controller = MenuController.FindFor(transform);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Register();

            if (m_controller)
            {
                m_controller.OnOpenMenu          += HandleScreenChanged;
                m_controller.OnOpenSiblingMenu   += HandleScreenChanged;
                m_controller.OnBack              += HandleScreenChanged;
                m_controller.OnStackEmptyChanged += HandleStackEmptyChanged;
            }

            UpdateVisibilityForCurrent();
        }

        protected void OnDisable()
        {
            if (m_controller)
            {
                m_controller.OnOpenMenu          -= HandleScreenChanged;
                m_controller.OnOpenSiblingMenu   -= HandleScreenChanged;
                m_controller.OnBack              -= HandleScreenChanged;
                m_controller.OnStackEmptyChanged -= HandleStackEmptyChanged;
            }

            Unregister();
        }

        void HandleScreenChanged(MenuScreen _)
            => UpdateVisibilityForCurrent();

        void HandleStackEmptyChanged(bool isEmpty)
            => Toggle(false);

        void UpdateVisibilityForCurrent()
        {
            var cur = m_controller ? m_controller.Current : null;
            Toggle(ShouldShowFor(cur));
        }

        bool ShouldShowFor(MenuScreen screen)
            => screen && s_lookup.TryGetValue(screen, out var g) && g == this;

        void Register()
        {
            foreach (var page in m_pages)
            {
                if (!page) continue;
                s_lookup[page] = this;
            }
        }

        void Unregister()
        {
            foreach (var page in m_pages)
            {
                if (!page) continue;
                if (s_lookup.TryGetValue(page, out var g) && g == this)
                    s_lookup.Remove(page);
            }
        }

        public static MenuSiblingGroup FindFor(MenuScreen screen)
        {
            if (!screen) return null;
            return s_lookup.TryGetValue(screen, out var g) ? g : null;
        }

        public int IndexOf(MenuScreen screen) => m_pages.IndexOf(screen);

        public MenuScreen Next(MenuScreen current)
        {
            var i = IndexOf(current);
            if (i < 0 || m_pages.Count == 0) return null;
            var next = i + 1;
            if (next >= m_pages.Count) next = m_wrap ? 0 : m_pages.Count - 1;
            return m_pages[next];
        }

        public MenuScreen Prev(MenuScreen current)
        {
            var i = IndexOf(current);
            if (i < 0 || m_pages.Count == 0) return null;
            var prev = i - 1;
            if (prev < 0) prev = m_wrap ? m_pages.Count - 1 : 0;
            return m_pages[prev];
        }
    }
}
