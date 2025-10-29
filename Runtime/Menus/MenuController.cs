// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
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
    /// Drives a shared selection indicator. Input *mode* (Pointer vs Navigation)
    /// is controlled externally (e.g., by InputManager via GameEvent).
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Controller")]
    [RequireComponent(typeof(CanvasGroup))]
    public class MenuController : MonoBehaviour
    {
        public enum UiInputMode { Pointer, Navigation }

        [Header("Input Mode (driven by InputManager/GameEvent)")]
        [SerializeField, Tooltip("Initial mode if we can't sync from InputManager on enable.")]
        UiInputMode m_initialMode = UiInputMode.Pointer;

        [SerializeField, Tooltip("Hide the selection indicator and clear selection in Pointer mode.")]
        bool m_hideSelectionInPointerMode = true;

        [SerializeField, Tooltip("If true, still select the first item when opening in Pointer mode.")]
        bool m_selectFirstOnOpenInPointerMode = false;
        

        [SerializeField, Tooltip("Optional field. If this Bool Variable is provided and is true...")]
        BoolReference m_BV_DeviceTypeGamepad;

        UiInputMode m_currentUiInputMode;
        public UiInputMode CurrentUiInputMode => m_currentUiInputMode;
        public event Action<UiInputMode> OnInputModeChanged;
        bool IsPointerMode => m_currentUiInputMode == UiInputMode.Pointer;


        [Header("Navigation (Cancel / Back)")]
        [Tooltip("If assigned, this action will be used for Cancel/Back. If not, this will fall back to the UI Input Module's Cancel action.")]
        [SerializeField] InputActionReference m_cancelActionOverride;

        [Tooltip("If false, pressing Cancel at the bottom of the stack does nothing (e.g., a Main Menu). If true, the last screen can be closed (e.g., a Pause menu).")]
        [SerializeField] bool m_allowClosingLastMenu = true;
        
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
        
        InputAction m_boundCancelAction;

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


#region MonoBehaviour Messages
        
        void Awake()
        {
            if (!m_canvasGroup)
            {
                m_canvasGroup = GetComponent<CanvasGroup>();
                
                // Start hidden. If a child screen is visible at Start(), it will be shown.
                m_canvasGroup.SetVisible(false);
            }

            // Seed a safe starting mode
            m_currentUiInputMode = m_initialMode;
        }
        
        void OnEnable()
        {
            var action = ResolveCancelAction();
            if (action != null)
            {
                m_boundCancelAction = action;
                m_boundCancelAction.performed += OnCancelAction;
            }

            SetUiInputMode(m_initialMode, immediate: true);
        }

        void OnDisable()
        {
            if (m_boundCancelAction != null)
                m_boundCancelAction.performed -= OnCancelAction;
            m_boundCancelAction = null;
        }
        
        void Start()
        {
            // If something else already opened a menu before Start(), don't override it.
            if (m_stack.Count > 0) return;

            
            var candidate = FindFirstVisibleChildScreen();
            if (candidate)
            {
                // This will re-call Show() on the screen and (by default) focus its first Selectable.
                MenuNav_OpenMenu(candidate, raiseEvent: false);
            }
            else
            {
                // Ensure the indicator hides if nothing is adopted.
                UpdateIndicatorActiveState();
            }
        }
        
        void LateUpdate()
        {
            if (!m_selectionIndicatorRect) return;
            
            if (m_BV_DeviceTypeGamepad != null)
                SetUiInputMode(m_BV_DeviceTypeGamepad ? UiInputMode.Navigation : UiInputMode.Pointer);

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
        
#endregion

#region Public Methods

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
        
        /// <summary>Open a new menu on top of the stack (push).</summary>
        public void MenuNav_OpenMenu(MenuScreen screen, bool raiseEvent = true)
        {
            if (!screen) return;
            
            var oldCount = m_stack.Count;

            if (m_stack.Count == 0 && m_pauseTimeScale)
            {
                m_prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
            }

            if (Current) Current.Hide();

            m_stack.Push(screen);
            screen.Show(); // Show() may select first; we'll reconcile below.

            if (raiseEvent) screen.OnOpenEvent();

            EnsureInitialSelectionBasedOnMode();
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
            screen.OnOpenEvent();
            EnsureInitialSelectionBasedOnMode();
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
            top.OnCloseEvent();
            OnBack?.Invoke(top);

            if (m_stack.Count > 0)
            {
                m_stack.Peek().Show();
                EnsureInitialSelectionBasedOnMode();
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
                top.OnCloseEvent();
                OnBack?.Invoke(top);
            }

            if (m_pauseTimeScale)
                Time.timeScale = m_prevTimeScale;

            m_forceIndicatorInstant = true;
            UpdateIndicatorActiveState();
            NotifyCountChange(oldCount);
        }

        /// <summary>
        /// Restores Time.timeScale to its previous value if it was paused by this MenuController.
        /// Normally this is handled automatically when the last menu is closed.
        /// However, this can be useful to call when transitioning scenes (say from gameplay to main menu)
        /// in cases where you don't want to dismiss all menus before a scene transition.
        /// </summary>
        public void RestoreTimeScale()
        {
            if (m_pauseTimeScale)
                Time.timeScale = m_prevTimeScale;
        }
        
#endregion

#region Helper Methods

        void SetUiInputMode(UiInputMode mode, bool immediate = false)
        {
            if (m_currentUiInputMode == mode)
                return;

            m_currentUiInputMode = mode;
            if (m_currentUiInputMode == UiInputMode.Pointer)
            {
                if (m_hideSelectionInPointerMode)
                    ClearSelectionUnderCurrent();

                UpdateIndicatorActiveState();
            }
            else // Navigation
            {
                EnsureSelectionAndSnap();
                if (immediate) m_forceIndicatorInstant = true;
            }

            OnInputModeChanged?.Invoke(m_currentUiInputMode);
        }

        InputAction ResolveCancelAction()
        {
            if (m_cancelActionOverride && m_cancelActionOverride.action != null)
                return m_cancelActionOverride.action;

            var es = EventSystem.current;
            var ui = es ? es.currentInputModule as InputSystemUIInputModule : null;
            return ui ? ui.cancel.action : null;
        }

        void OnCancelAction(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;
            if (Current == null) return;

            // Give the current screen the first chance to handle/consume Cancel.
            if (Current.OnCancelPressed())
                return;

            // Otherwise, check with the higher level MenuController rules.
            BackOneWithLastMenuGuard();
        }

        void BackOneWithLastMenuGuard()
        {
            if (m_stack.Count == 0)
                return;

            // If there is more than one screen, always pop.
            if (m_stack.Count > 1)
            {
                MenuNav_BackOneMenu();
                return;
            }

            // If we're here, we are at the root. Only pop if allowed.
            if (m_allowClosingLastMenu)
                MenuNav_BackOneMenu();
            
            // Otherwise, do nothing (such as if we're on a Main Menu and shouldn't back out further)
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

            // Hide the indicator in pointer mode
            if (m_hideSelectionInPointerMode && IsPointerMode)
                shouldShow = false;
            
            if (m_selectionIndicatorRect.gameObject.activeSelf != shouldShow)
                m_selectionIndicatorRect.gameObject.SetActive(shouldShow);
        }
        
        // Ensure selection based on input mode before snapping.
        void EnsureInitialSelectionBasedOnMode()
        {
            if (IsPointerMode && !m_selectFirstOnOpenInPointerMode)
            {
                ClearSelectionUnderCurrent();
                UpdateIndicatorActiveState();
            }
            else
            {
                EnsureSelectionAndSnap();
                m_forceIndicatorInstant = true;
            }
        }

        void ClearSelectionUnderCurrent()
        {
            var es = EventSystem.current;
            if (!es || !Current) return;

            var selGO = es.currentSelectedGameObject;
            if (selGO && selGO.transform.IsChildOf(Current.transform))
                es.SetSelectedGameObject(null);
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
        
#endregion

    }
}
