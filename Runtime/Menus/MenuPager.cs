// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Buck
{
    /// <summary>
    /// Pager that renders a horizontal list of sibling page titles and underlines the selected one.
    /// Contains no Selectables so it doesn't affect input focus.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("BUCK/UI/Menu Pager (Bumper Bar)")]
    public class MenuPager : MonoBehaviour
    {
        [Header("Wiring")]
        [SerializeField] MenuController m_controller;
        [SerializeField, Tooltip("Parent RectTransform that will contain the generated page items (ideally has a HorizontalLayoutGroup).")]
        RectTransform m_itemsRoot;
        [SerializeField, Tooltip("Underline bar RectTransform that will be reparented to the selected item.")]
        RectTransform m_underlineRect;
        [SerializeField, Tooltip("Optional: action that triggers PrevPage (e.g., keyboard [ or Shift+Tab).")]
        InputActionReference m_prevPageAction;
        [SerializeField, Tooltip("Optional: action that triggers NextPage (e.g., keyboard ] or Tab).")]
        InputActionReference m_nextPageAction;

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
            public LayoutElement Layout;
        }

        readonly List<Item> m_items = new();
        MenuSiblingGroup m_builtGroup;
        Item m_selectedItem;
        CanvasGroup m_canvasGroup;

        void Awake()
        {
            if (!m_controller)
                m_controller = MenuController.FindFor(transform);
            m_canvasGroup = GetComponent<CanvasGroup>();
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
            
            // Bind input actions
            if (m_prevPageAction && m_prevPageAction.action != null)
                m_prevPageAction.action.performed += OnPrevAction;

            if (m_nextPageAction && m_nextPageAction.action != null)
                m_nextPageAction.action.performed += OnNextAction;
            
            Refresh();
        }

        void OnDisable()
        {
            if (!m_controller) return;

            m_controller.OnOpenMenu          -= HandleScreenChanged;
            m_controller.OnOpenSiblingMenu   -= HandleScreenChanged;
            m_controller.OnBack              -= HandleScreenChanged;
            m_controller.OnStackEmptyChanged -= HandleStackEmptyChanged;
            
            // Unbind input actions
            if (m_prevPageAction && m_prevPageAction.action != null)
                m_prevPageAction.action.performed -= OnPrevAction;

            if (m_nextPageAction && m_nextPageAction.action != null)
                m_nextPageAction.action.performed -= OnNextAction;
        }
        
        void HandleScreenChanged(MenuScreen _) => Refresh();

        void HandleStackEmptyChanged(bool isEmpty)
        {
            var show = !isEmpty && ShouldShowFor(m_controller?.Current);
            if (m_canvasGroup) m_canvasGroup.SetVisible(show);
        }

        bool ShouldShowFor(MenuScreen screen)
        {
            if (!screen) return false;
            var group = MenuSiblingGroup.FindFor(screen);
            return group != null && group.Pages.Count > 1;
        }
        
        void OnPrevAction(InputAction.CallbackContext ctx) { if (ctx.performed) PrevPage(); }
        void OnNextAction(InputAction.CallbackContext ctx) { if (ctx.performed) NextPage(); }

        public void NextPage()
        {
            var cur = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var next = group?.Next(cur);
            if (next) m_controller.MenuNav_OpenSiblingMenu(next); // replace top, keep parent
        }

        public void PrevPage()
        {
            var cur = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);
            var prev = group?.Prev(cur);
            if (prev) m_controller.MenuNav_OpenSiblingMenu(prev);
        }

        void Refresh()
        {
            var cur   = m_controller ? m_controller.Current : null;
            var group = MenuSiblingGroup.FindFor(cur);

            bool show = group != null && group.Pages.Count > 1;
            if (m_canvasGroup) m_canvasGroup.SetVisible(show);
            if (!show) return;

            if (!m_itemsRoot || !m_underlineRect) return;

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

                var layoutElement = containerGO.AddComponent<LayoutElement>();
                if (m_itemHeight > 0f) layoutElement.minHeight = layoutElement.preferredHeight = m_itemHeight;

                // Label
                TextMeshProUGUI label = m_itemPrototype
                    ? Instantiate(m_itemPrototype, container)
                    : CreateDefaultLabel(container);

                label.raycastTarget = false;
                
                // Bind label (localized if available) and compute width
                BindLabelForPage(label, page, layoutElement);
                AddPointerHitTarget(containerGO, page);

                m_items.Add(new Item
                {
                    Screen = page,
                    Container = container,
                    Label = label,
                    Layout = layoutElement
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

        void AddPointerHitTarget(GameObject containerGO, MenuScreen page)
        {
            // Ensure a raycast target exists (transparent Image is fine).
            var graphic = containerGO.GetComponent<Graphic>();
            if (!graphic)
            {
                var img = containerGO.AddComponent<Image>();
                img.color = new Color(0, 0, 0, 0); // fully transparent
                img.raycastTarget = true;
            }
            else
            {
                graphic.raycastTarget = true;
            }

            var trigger = containerGO.GetComponent<EventTrigger>() ?? containerGO.AddComponent<EventTrigger>();

            // Click = open that page as a sibling
            AddTrigger(trigger, EventTriggerType.PointerClick, (BaseEventData data) =>
            {
                var ped = (PointerEventData)data;
                if (ped.button != PointerEventData.InputButton.Left) return;

                if (m_controller && page)
                    m_controller.MenuNav_OpenSiblingMenu(page); // sibling switch, parent preserved

                EventSystem.current?.SetSelectedGameObject(null); // keep selection indicator off the pager
            });

            // Optional: mouse wheel cycles while hovering
            AddTrigger(trigger, EventTriggerType.Scroll, (BaseEventData data) =>
            {
                var ped = (PointerEventData)data;
                if (ped.scrollDelta.y > 0f) PrevPage();
                else if (ped.scrollDelta.y < 0f) NextPage();
            });
        }

        static void AddTrigger(EventTrigger trigger, EventTriggerType type, UnityAction<BaseEventData> cb)
        {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(cb);
            trigger.triggers.Add(entry);
        }

        void UpdateSelected(MenuSiblingGroup group, MenuScreen current)
        {
            Item selected = null;
            var index = group.IndexOf(current);
            if (index >= 0 && index < m_items.Count)
                selected = m_items[index];

            foreach (var it in m_items)
            {
                var lblRt = it.Label.rectTransform;
                lblRt.anchoredPosition = it == selected
                    ? new Vector2(0f, m_selectedYOffset)
                    : Vector2.zero;
            }

            if (selected != null)
            {
                m_underlineRect.SetParent(selected.Container, worldPositionStays: false);
                m_underlineRect.anchorMin = new Vector2(0.5f, 0f);
                m_underlineRect.anchorMax = new Vector2(0.5f, 0f);
                m_underlineRect.pivot     = new Vector2(0.5f, 0.5f);
                m_underlineRect.anchoredPosition = new Vector2(0f, m_underlineVerticalOffset);

                selected.Label.ForceMeshUpdate();
                float width = selected.Label.preferredWidth + (m_underlineHorizontalPadding * 2f);
                m_underlineRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);

                var g = m_underlineRect.GetComponent<Graphic>();
                if (g) g.raycastTarget = false;
                
                m_underlineRect.SetAsLastSibling();

                var title = current?.TitleText;
                m_onTitleChanged?.Invoke(title ?? string.Empty);
            }

            m_selectedItem = selected;
        }
        
        void BindLabelForPage(TextMeshProUGUI label, MenuScreen page, LayoutElement layout)
        {
            if (!page || !label || !layout) return;

            void OnTextChanged(string s)
            {
                label.ForceMeshUpdate();
                var pw = label.preferredWidth;
                if (pw > 0f)
                {
                    layout.minWidth = layout.preferredWidth = pw;
                    LayoutRebuilder.MarkLayoutForRebuild(m_itemsRoot);
                }

                if (m_selectedItem != null && m_selectedItem.Label == label)
                {
                    UpdateUnderlineWidthForSelected();
                    m_onTitleChanged?.Invoke(s ?? string.Empty);
                }
            }

            page.Title.BindTo(label, OnTextChanged);

            label.ForceMeshUpdate();
            var initW = label.preferredWidth;
            if (initW > 0f) layout.minWidth = layout.preferredWidth = initW;
        }
                
        void UpdateUnderlineWidthForSelected()
        {
            if (m_selectedItem == null || m_underlineRect == null) return;
            m_selectedItem.Label.ForceMeshUpdate();
            float width = m_selectedItem.Label.preferredWidth + (m_underlineHorizontalPadding * 2f);
            m_underlineRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
    }
}
