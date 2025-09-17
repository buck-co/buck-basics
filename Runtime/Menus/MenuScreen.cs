// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Common behavior for a UI "screen": show/hide and auto-binding via VariableBinding components.
    /// The selection indicator is handled by MenuController.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("BUCK/UI/Menu Screen")]
    public class MenuScreen : MonoBehaviour
    {
        [Tooltip("Should this screen be visible when enabled?")]
        [SerializeField] protected bool m_startVisible = false;

        CanvasGroup m_canvasGroup;

        protected virtual void Reset()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Awake()
        {
            if (!m_canvasGroup)
                m_canvasGroup = GetComponent<CanvasGroup>();
            AutoBindFromChildren();
        }

        protected virtual void OnEnable()
        {
            if (m_startVisible)
                Show(focusFirst: true);
            else
                Hide();
        }

        /// <summary>Convenience method to find the nearest MenuController and open this menu.</summary>
        public virtual void MenuNav_OpenThisMenu()
            => MenuController.FindFor(transform)?.MenuNav_OpenMenu(this);
        
        /// <summary>Convenience method to find the nearest MenuController and open the specified menu.</summary>
        public virtual void MenuNav_OpenMenu(MenuScreen screen)
            => MenuController.FindFor(transform)?.MenuNav_OpenMenu(screen);
        
        /// <summary>Convenience method to find the nearest MenuController and open this menu as a sibling of the current menu.</summary>
        public virtual void MenuNav_OpenThisMenuAsSiblingMenu()
            => MenuController.FindFor(transform)?.MenuNav_OpenSiblingMenu(this);
        
        /// <summary>Convenience method to find the nearest MenuController and open the specified menu as a sibling of the current menu.</summary>
        public virtual void MenuNav_OpenSiblingMenu(MenuScreen screen)
            => MenuController.FindFor(transform)?.MenuNav_OpenSiblingMenu(screen);
        
        /// <summary>Convenience method to find the nearest MenuController and go back one menu.</summary>
        public virtual void MenuNav_BackOneMenu()
            => MenuController.FindFor(transform)?.MenuNav_BackOneMenu();
        
        /// <summary>Convenience method to find the nearest MenuController and close all menus.</summary>
        public virtual void MenuNav_CloseAllMenus()
            => MenuController.FindFor(transform)?.MenuNav_CloseAllMenus();
        

        /// <summary>
        /// Show this screen and focus the first Selectable child.
        /// </summary>
        public virtual void Show(bool focusFirst = true)
        {
            m_canvasGroup.SetVisible(true);

            // Always pull fresh values from variables when a screen is shown.
            RefreshBindingsInChildren();

            if (focusFirst)
            {
                var first = FindFirstSelectable();
                if (first)
                    EventSystem.current?.SetSelectedGameObject(first.gameObject);
            }
        }

        /// <summary>
        /// Hide this screen.
        /// </summary>
        public virtual void Hide()
            => m_canvasGroup.SetVisible(false);

        /// <summary>
        /// Set visibility.
        /// </summary>
        public void Toggle(bool visible)
        {
            if (visible)
                Show();
            else
                Hide();
        }

        /// <summary>
        /// True if this screen is interactable and visible.
        /// </summary>
        protected bool IsVisible()
            => m_canvasGroup && m_canvasGroup.interactable && m_canvasGroup.blocksRaycasts && m_canvasGroup.alpha > 0.0f;

        void AutoBindFromChildren()
        {
            var bindings = GetComponentsInChildren<VariableBinding>(true);
            foreach (var b in bindings)
            {
                var selectable = b.Selectable;
                var variable   = b.Variable;
                var raiseEvent = b.RaiseGameEventOnChange;
                if (!selectable || !variable)
                {
                    Debug.LogError($"VariableBinding on '{b.gameObject.name}' missing Selectable or Variable.", b);
                    continue;
                }
                AttachHelper(selectable, variable, raiseEvent);
            }
        }
        
        /// <summary>
        /// Attach the correct UI helper given a Selectable + BaseVariable pair.
        /// </summary>
        static void AttachHelper(Selectable selectable, BaseVariable variable, bool raiseEvent)
        {
            if (!selectable || !variable)
                return;

            switch (selectable)
            {
                case Slider slider when variable is FloatVariable fv:
                {
                    var h = slider.GetComponent<UISliderHelper>() ?? slider.gameObject.AddComponent<UISliderHelper>();
                    h.SetVariable(fv, raiseEvent);
                    h.Initialize();
                    break;
                }
                case Toggle toggle when variable is BoolVariable bv:
                {
                    var h = toggle.GetComponent<UIToggleHelper>() ?? toggle.gameObject.AddComponent<UIToggleHelper>();
                    h.SetVariable(bv, raiseEvent);
                    h.Initialize();
                    break;
                }
                default:
                    Debug.LogError($"UIBinderUtility - Unsupported pair: {variable.GetType().Name} with {selectable.GetType().Name}", selectable);
                    break;
            }
        }

        /// <summary>
        /// Pulls the latest values from all bound variables into their UI controls (no notifications).
        /// </summary>
        protected void RefreshBindingsInChildren()
        {
            var binders = GetComponentsInChildren<IUIValueBinder>(true);
            foreach (var b in binders)
                b.RefreshFromVariable();
        }

        /// <summary>
        /// Find the first active & interactable Selectable under this screen in hierarchy order.
        /// </summary>
        public Selectable FindFirstSelectable()
        {
            // Breadth-first traversal for predictable hierarchy order.
            var queue = new Queue<Transform>();
            queue.Enqueue(transform);

            while (queue.Count > 0)
            {
                var t = queue.Dequeue();

                if (t.TryGetComponent<Selectable>(out var sel))
                {
                    if (sel.IsActive() && sel.interactable)
                        return sel;
                }

                for (int i = 0; i < t.childCount; i++)
                    queue.Enqueue(t.GetChild(i));
            }

            // Fallback to any child (even inactive), just to avoid dead-ends in misconfigured UIs
            return GetComponentInChildren<Selectable>(true);
        }
    }
}
