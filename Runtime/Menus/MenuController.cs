// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buck
{
    public enum IndicatorXMode
    {
        KeepCurrentX,
        MatchItemCenterX,
        LeftOfItemWithPadding,
        RightOfItemWithPadding
    }

    /// <summary>
    /// Manages a stack of MenuScreens. Top is active. Optional timeScale pause.
    /// Raises events when the stack becomes empty/non-empty.
    /// Also drives a shared selection indicator for all screens under this controller.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Controller")]
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuController : MonoBehaviour
    {
        [Header("Behavior")]
        [Tooltip("Automatically set Time.timeScale = 0 when the first menu opens, and restores when all menus close.")]
        [SerializeField] bool m_pauseTimeScale = true;

        [Header("Selection Indicator (optional)")]
        [Tooltip("Indicator graphic to follow the currently selected UI element.")]
        [SerializeField] RectTransform m_selectionIndicatorRect;

        [Tooltip("Unscaled smoothing time for indicator movement.")]
        [SerializeField] float m_indicatorSmoothTime = 0.05f;

        [Tooltip("How to compute the indicator X position relative to the selected item.")]
        [SerializeField] IndicatorXMode m_indicatorXMode = IndicatorXMode.LeftOfItemWithPadding;

        [Tooltip("Padding used by LeftOf/RightOf modes (in pixels).")]
        [SerializeField] float m_indicatorXPadding = 24f;
        
        protected CanvasGroup m_canvasGroup;

        readonly Stack<MenuScreen> m_stack = new();
        float m_prevTimeScale = 1f;

        // indicator state
        Vector3 m_indicatorWorldVelocity;
        bool m_forceIndicatorInstant;

        public MenuScreen Current => m_stack.Count > 0 ? m_stack.Peek() : null;

        // Events
        public event Action<MenuScreen> OnOpenMenu;
        public event Action<MenuScreen> OnBack;
        public event Action<MenuScreen> OnOpenSiblingMenu;
        public event Action<bool> OnStackEmptyChanged;

        void Awake()
        {
            if (!m_canvasGroup)
            {
                m_canvasGroup = GetComponent<CanvasGroup>();
                
                // Start hidden. If a child screen is visible at Start(), it will be shown.
                m_canvasGroup.SetVisible(false);
            }
        }
        
        void Start()
        {
            // If something else already opened a menu before Start(), don't override it.
            if (m_stack.Count > 0) return;

            
            var candidate = FindFirstVisibleChildScreen();
            if (candidate)
            {
                // This will re-call Show() on the screen, which is safe and ensures selection reticle logic runs.
                MenuNav_OpenMenu(candidate);
            }
            else
            {
                // Ensure the indicator hides if nothing is adopted.
                UpdateIndicatorActiveState();
            }
        }

        /// <summary>
        /// Find the first MenuScreen under this controller that is currently visible
        /// (CanvasGroup alpha>0, interactable, blocksRaycasts, and active in hierarchy).
        /// </summary>
        MenuScreen FindFirstVisibleChildScreen()
        {
            var screens = GetComponentsInChildren<MenuScreen>(includeInactive: true);
            foreach (var s in screens)
            {
                if (!s.isActiveAndEnabled) continue; // component enabled and hierarchy-active
                var canvasGroup = s.GetComponent<CanvasGroup>();
                if (canvasGroup != null && canvasGroup.alpha > 0f && canvasGroup.interactable && canvasGroup.blocksRaycasts)
                    return s;
            }
            return null;
        }
        
        /// <summary>
        /// Find the nearest MenuController parent for a given Transform (fallbacks to scene search).
        /// </summary>
        public static MenuController FindFor(Transform t)
        {
            if (t)
            {
                var parentController = t.GetComponentInParent<MenuController>();
                if (parentController)
                    return parentController;
            }
            
            // TODO: Is this fallback a good idea? It could lead to surprising behavior if there are multiple controllers in the scene.
            return FindFirstObjectByType<MenuController>();
        }

        /// <summary>
        /// Open a new menu on top of the stack (push).
        /// </summary>
        public void MenuNav_OpenMenu(MenuScreen screen)
        {
            if (!screen) return;
            
            var oldCount = m_stack.Count;

            // If this is the first menu and we're pausing timeScale
            // then save the current timeScale and set to 0.
            if (m_stack.Count == 0 && m_pauseTimeScale)
            {
                m_prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }

            if (Current)
                Current.Hide();
            
            m_stack.Push(screen);
            screen.Show();
            EnsureSelectionAndSnap();
            m_forceIndicatorInstant = true;

            UpdateIndicatorActiveState();
            OnOpenMenu?.Invoke(screen);
            NotifyCountChange(oldCount);
        }

        /// <summary>
        /// Open a sibling menu at the same depth (replace top of stack). Back goes to the parent.
        /// This is useful for switching between tabs or modes within a single conceptual menu.
        /// </summary>
        public void MenuNav_OpenSiblingMenu(MenuScreen screen)
        {
            if (!screen) return;
            
            var oldCount = m_stack.Count;

            if (Current)
            {
                var old = m_stack.Pop();
                old.Hide();
                OnBack?.Invoke(old);
            }

            // If this is the first menu and we're pausing timeScale
            // then save the current timeScale and set to 0.
            if (m_stack.Count == 0 && m_pauseTimeScale)
            {
                m_prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }

            m_stack.Push(screen);
            screen.Show();
            EnsureSelectionAndSnap();
            m_forceIndicatorInstant = true;

            UpdateIndicatorActiveState();
            OnOpenSiblingMenu?.Invoke(screen);
            NotifyCountChange(oldCount);
        }

        /// <summary>
        /// Go back one level (pop).
        /// </summary>
        public void MenuNav_BackOneMenu()
        {
            if (m_stack.Count == 0) return;
            
            var oldCount = m_stack.Count;

            var top = m_stack.Pop();
            top.Hide();
            OnBack?.Invoke(top);

            if (m_stack.Count > 0)
            {
                m_stack.Peek().Show();
                EnsureSelectionAndSnap();
            }
            else if (m_pauseTimeScale)
            {
                Time.timeScale = m_prevTimeScale;
            }

            m_forceIndicatorInstant = true;
            UpdateIndicatorActiveState();
            NotifyCountChange(oldCount);
        }

        /// <summary>
        /// Close all menus (clear stack).
        /// </summary>
        public void MenuNav_CloseAllMenus()
        {
            if (m_stack.Count == 0) return;
            
            var oldCount = m_stack.Count;

            while (m_stack.Count > 0)
            {
                var top = m_stack.Pop();
                top.Hide();
                OnBack?.Invoke(top);
            }

            if (m_pauseTimeScale)
                Time.timeScale = m_prevTimeScale;

            m_forceIndicatorInstant = true;
            UpdateIndicatorActiveState();
            NotifyCountChange(oldCount);
        }

        void NotifyCountChange(int oldCount)
        {
            bool wasEmpty = oldCount == 0;
            bool isEmpty  = m_stack.Count == 0;
            
            // Set the CanvasGroup visibility of this MenuController
            m_canvasGroup.SetVisible(!isEmpty);
            
            if (wasEmpty != isEmpty)
                OnStackEmptyChanged?.Invoke(isEmpty);
        }

        void UpdateIndicatorActiveState()
        {
            if (!m_selectionIndicatorRect)return;
            
            bool shouldShow = m_stack.Count > 0 && EventSystem.current &&
                              EventSystem.current.currentSelectedGameObject;

            // Also require the selected object to be under the current screen (prevents showing on unrelated UI)
            if (shouldShow && Current)
            {
                var selected = EventSystem.current.currentSelectedGameObject.transform;
                shouldShow = selected.IsChildOf(Current.transform);
            }

            if (m_selectionIndicatorRect.gameObject.activeSelf != shouldShow)
                m_selectionIndicatorRect.gameObject.SetActive(shouldShow);
        }
        
        // Ensures the EventSystem has a valid selection under the current screen,
        // then snaps the indicator immediately to that element (world space).
        void EnsureSelectionAndSnap()
        {
            if (!m_selectionIndicatorRect || Current == null) return;

            var eventSystem = EventSystem.current;
            Selectable sel = null;

            // If EventSystem has a selection AND it's under the current screen, use it.
            var currentGO = eventSystem ? eventSystem.currentSelectedGameObject : null;
            if (currentGO && currentGO.transform.IsChildOf(Current.transform))
                sel = currentGO.GetComponent<Selectable>();

            // Otherwise, pick the first selectable on this screen and set selection.
            if (sel == null)
            {
                sel = Current.FindFirstSelectable();
                if (sel) eventSystem?.SetSelectedGameObject(sel.gameObject);
            }

            // Toggle visibility based on the final selection state.
            UpdateIndicatorActiveState();

            // Snap the indicator immediately (no waiting for LateUpdate/input).
            SnapIndicatorTo(sel, instant: true);
        }

        // Compute the indicator target from a Selectable's RectTransform in world space,
        // apply X-mode and padding, and set the position (instant or smoothed).
        void SnapIndicatorTo(Selectable sel, bool instant)
        {
            if (!m_selectionIndicatorRect || sel == null) return;

            var rt = sel.GetComponent<RectTransform>();
            if (!rt) return;

            var corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            var left   = corners[0];
            var right  = corners[2];
            var center = 0.5f * (left + right);

            float targetX = m_selectionIndicatorRect.position.x;
            switch (m_indicatorXMode)
            {
                case IndicatorXMode.MatchItemCenterX:      targetX = center.x; break;
                case IndicatorXMode.LeftOfItemWithPadding: targetX = left.x  - m_indicatorXPadding; break;
                case IndicatorXMode.RightOfItemWithPadding:targetX = right.x + m_indicatorXPadding; break;
                case IndicatorXMode.KeepCurrentX:
                default: break;
            }

            var target = new Vector3(targetX, center.y, m_selectionIndicatorRect.position.z);

            if (instant)
            {
                m_indicatorWorldVelocity = Vector3.zero;
                m_selectionIndicatorRect.position = target;
                m_forceIndicatorInstant = false; // we've already snapped
            }
            else
            {
                // Let LateUpdate smooth to the target
                m_forceIndicatorInstant = true;
            }
        }

        void LateUpdate()
        {
            if (!m_selectionIndicatorRect) return;

            // Keep visibility correct even if nobody called Open/Back this frame
            UpdateIndicatorActiveState();
            if (!m_selectionIndicatorRect.gameObject.activeInHierarchy) return;
            if (!Current) return;

            var selectedRT = EventSystem.current?.currentSelectedGameObject?.GetComponent<RectTransform>();
            if (!selectedRT) return;
            
            // Compute world-space center/edges of the selected item
            var corners = new Vector3[4];
            selectedRT.GetWorldCorners(corners);
            var left  = corners[0];
            var right = corners[2];
            var center = 0.5f * (left + right);

            // Choose target X in world space based on mode
            float targetX = m_selectionIndicatorRect.position.x;
            switch (m_indicatorXMode)
            {
                case IndicatorXMode.MatchItemCenterX:
                    targetX = center.x;
                    break;
                case IndicatorXMode.LeftOfItemWithPadding:
                    targetX = left.x - m_indicatorXPadding;
                    break;
                case IndicatorXMode.RightOfItemWithPadding:
                    targetX = right.x + m_indicatorXPadding;
                    break;
                case IndicatorXMode.KeepCurrentX:
                default:
                    // do nothing
                    break;
            }

            // Target world position: keep current Z
            var target = new Vector3(targetX, center.y, m_selectionIndicatorRect.position.z);

            if (m_forceIndicatorInstant)
            {
                m_indicatorWorldVelocity = Vector3.zero;
                m_selectionIndicatorRect.position = target;
                m_forceIndicatorInstant = false;
                return;
            }

            // Smooth in unscaled time (works while paused)
            m_selectionIndicatorRect.position = Vector3.SmoothDamp(
                m_selectionIndicatorRect.position,
                target,
                ref m_indicatorWorldVelocity,
                m_indicatorSmoothTime,
                Mathf.Infinity,
                Time.unscaledDeltaTime
            );
        }
    }
}
