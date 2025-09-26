// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Pager that renders a horizontal list of sibling page titles and underlines the selected one.
    /// Contains no Selectables so it doesn't affect input focus.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Pager (Bumper Bar)")]
    public class MenuPager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] MenuController m_controller;
        [SerializeField, Tooltip("Parent RectTransform that will contain the generated page items (ideally has a HorizontalLayoutGroup).")]
        RectTransform m_itemsRoot;
        [SerializeField, Tooltip("Underline bar RectTransform that will be reparented to the selected item.")]
        RectTransform m_underlineRect;

        [Header("Item Template (optional)")]
        [SerializeField, Tooltip("If assigned, this TextMeshProUGUI will be cloned for each page; otherwise a default will be created.")]
        TextMeshProUGUI m_itemPrototype;
        [SerializeField, Tooltip("Optional fixed item height for layout; 0 means auto.")]
        float m_itemHeight = 0f;

        [Header("Visuals")]
        [SerializeField, Tooltip("How far the selected label moves up (in pixels).")]
        float m_selectedYOffset = 6f;
        [SerializeField, Tooltip("Extra horizontal padding added to underline width.")]
        float m_underlineHorizontalPadding = 2f;
        [SerializeField, Tooltip("Vertical offset for the underline relative to the bottom of the item container.")]
        float m_underlineVerticalOffset = 0f;

        [Header("Events (optional)")]
        [SerializeField] UnityEvent<string> m_onTitleChanged;

        class Item
        {
            public MenuScreen Screen;
            public RectTransform Container;
            public TextMeshProUGUI Label;
        }

        readonly List<Item> m_items = new();
        MenuSiblingGroup m_builtGroup;
        Item m_selectedItem;

        void Awake()
        {
            if (!m_controller)
                m_controller = MenuController.FindFor(transform);
        }

        void OnEnable()
        {
            if (m_controller)
            {
                m_controller.OnOpenMenu          += HandleScreenChanged;
                m_controller.OnOpenSiblingMenu   += HandleScreenChanged;
                m_controller.OnBack              += HandleScreenChanged;
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

        void HandleScreenChanged(MenuScreen _) => Refresh();
        void HandleStackEmptyChanged(bool isEmpty) => gameObject.SetActive(!isEmpty && ShouldShowFor(m_controller?.Current));

        bool ShouldShowFor(MenuScreen screen)
        {
            if (!screen) return false;
            var group = MenuSiblingGroup.FindFor(screen);
            return group != null && group.Pages.Count > 1;
        }

        public void NextPage()
        {
            var cur   = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var next  = group?.Next(cur);
            if (next) m_controller.MenuNav_OpenSiblingMenu(next); // replace top, keep parent
        }

        public void PrevPage()
        {
            var cur   = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var prev  = group?.Prev(cur);
            if (prev) m_controller.MenuNav_OpenSiblingMenu(prev);
        }

        void Refresh()
        {
            var cur = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);

            bool show = group != null && group.Pages.Count > 1;
            gameObject.SetActive(show);
            if (!show || !m_itemsRoot || !m_underlineRect)
                return;

            if (m_builtGroup != group || m_items.Count != group.Pages.Count)
                RebuildItems(group);

            UpdateSelected(group, cur);
        }

        void RebuildItems(MenuSiblingGroup group)
        {
            // Preserve underline outside while we rebuild
            if (m_underlineRect && m_underlineRect.parent == m_itemsRoot)
                m_underlineRect.SetParent(transform, worldPositionStays: false);

            foreach (Transform child in m_itemsRoot)
                Destroy(child.gameObject);
            m_items.Clear();

            foreach (var page in group.Pages)
            {
                if (!page) continue;

                // Container for layout
                var containerGO = new GameObject($"Item ({page.name})", typeof(RectTransform));
                var container   = containerGO.GetComponent<RectTransform>();
                container.SetParent(m_itemsRoot, false);
                container.anchorMin = container.anchorMax = new Vector2(0.5f, 0.5f);
                container.pivot     = new Vector2(0.5f, 0.5f);

                var le = containerGO.AddComponent<LayoutElement>();
                if (m_itemHeight > 0f) le.minHeight = le.preferredHeight = m_itemHeight;

                // Label
                TextMeshProUGUI label = m_itemPrototype
                    ? Instantiate(m_itemPrototype, container)
                    : CreateDefaultLabel(container);

                label.raycastTarget = false;
                label.text = string.IsNullOrEmpty(page.TitleText) ? page.name : page.TitleText;

                // Ensure layout width fits text
                label.ForceMeshUpdate();
                float pw = label.preferredWidth;
                if (pw > 0f)
                {
                    le.minWidth = le.preferredWidth = pw;
                }

                m_items.Add(new Item
                {
                    Screen = page,
                    Container = container,
                    Label = label
                });
            }

            // Force a layout pass so preferred sizes are valid
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_itemsRoot);

            m_builtGroup = group;
        }

        TextMeshProUGUI CreateDefaultLabel(RectTransform parent)
        {
            var go = new GameObject("Label", typeof(RectTransform));
            var rt = go.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);

            var tmp = go.AddComponent<TextMeshProUGUI>();
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableWordWrapping = false;
            tmp.overflowMode = TextOverflowModes.Overflow;
            return tmp;
        }

        void UpdateSelected(MenuSiblingGroup group, MenuScreen current)
        {
            // Find selected item
            Item sel = null;
            var index = group.IndexOf(current);
            if (index >= 0 && index < m_items.Count)
                sel = m_items[index];

            // Update label offsets
            foreach (var it in m_items)
            {
                var lblRt = it.Label.rectTransform;
                lblRt.anchoredPosition = it == sel
                    ? new Vector2(0f, m_selectedYOffset)
                    : Vector2.zero;
            }

            // Reparent & size underline
            if (sel != null)
            {
                m_underlineRect.SetParent(sel.Container, worldPositionStays: false);
                m_underlineRect.anchorMin = new Vector2(0.5f, 0f);
                m_underlineRect.anchorMax = new Vector2(0.5f, 0f);
                m_underlineRect.pivot     = new Vector2(0.5f, 0.5f);
                m_underlineRect.anchoredPosition = new Vector2(0f, m_underlineVerticalOffset);

                sel.Label.ForceMeshUpdate();
                float width = sel.Label.preferredWidth + (m_underlineHorizontalPadding * 2f);
                m_underlineRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                // Keep height as authored on the underline rect
                m_underlineRect.SetAsLastSibling();

                var title = current?.TitleText;
                m_onTitleChanged?.Invoke(title ?? string.Empty);
            }

            m_selectedItem = sel;
        }
    }
}
