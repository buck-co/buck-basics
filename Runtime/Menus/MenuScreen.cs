// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Buck
{
    /// <summary>
    /// A UI "screen": show/hide, auto-binding via VariableBinding components,
    /// and focus management. The selection indicator is handled by MenuController.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Menu Screen")]
    public class MenuScreen : MenuView
    {
        [Header("Cancel / Back")]
        [SerializeField, Tooltip("If true, Cancel on this screen is consumed and the controller will not auto-back. " +
                                 "Use this for things like a Main Menu where the user should not back out any further.")]
        bool m_blockCancelOnThisScreen = false;
        
        [Tooltip("If true, when this screen is shown, the first Selectable child will be focused.")]
        [SerializeField] bool m_focusFirstOnShow = true;
        
        protected override void Awake()
        {
            base.Awake();
            AutoBindFromChildren();
        }

        protected override void OnEnable()
        {
            if (m_startVisible)
                MenuNav_OpenThisMenu();
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
        public override void Show(bool focusFirst = true)
        {
            base.Show(focusFirst: false);

            // Always pull fresh values from variables when a screen is shown.
            RefreshBindingsInChildren();

            if (focusFirst && m_focusFirstOnShow)
            {
                var first = FindFirstSelectable();
                if (first)
                    EventSystem.current?.SetSelectedGameObject(first.gameObject);
            }
        }

        public override void Hide()
        {
            CollapseDropdownsInChildren();
            base.Hide();
        }
        
        void CollapseDropdownsInChildren()
        {
            var dropdownComponents = GetComponentsInChildren<TMP_Dropdown>(true);
            foreach (var dropdown in dropdownComponents)
            {
                if (dropdown && dropdown.IsExpanded)
                {
                    // TMP_Dropdown.Hide() destroys the spawned "Dropdown List" and returns the control to a collapsed state.
                    dropdown.Hide();
                }
            }
        }

        /// <summary>
        /// Called by MenuController when UI/Cancel is pressed while this screen is current.
        /// Return true to consume the input (no auto-back). Return false to allow default back behavior.
        /// </summary>
        public virtual bool OnCancelPressed()
            => m_blockCancelOnThisScreen;
        
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

        protected void RefreshBindingsInChildren()
        {
            var binders = GetComponentsInChildren<IUIValueBinder>(true);
            foreach (var b in binders)
                b.RefreshFromVariable();
        }

        /// <summary>Find the first active & interactable Selectable under this screen in hierarchy order.</summary>
        public Selectable FindFirstSelectable()
        {
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

            return GetComponentInChildren<Selectable>(true);
        }
    }
}
