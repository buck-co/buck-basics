// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Raises a UnityEvent when the user activates this control (left click or Submit),
    /// as opposed to whenever its value changes. Wire interaction feedback such as SFX
    /// to this instead of onValueChanged, which also fires for programmatic changes
    /// (seeding values on menu setup, ToggleGroup registration, refreshing bindings).
    /// </summary>
    [AddComponentMenu("BUCK/UI/Interaction Event")]
    public class UIInteractionEvent : MonoBehaviour, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField, Tooltip("Raised when the user clicks or submits this control while it is interactable.")]
        UnityEvent m_onUserInteracted = new();

        public UnityEvent OnUserInteracted => m_onUserInteracted;

        Selectable m_selectable;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
                Raise();
        }

        public void OnSubmit(BaseEventData eventData) => Raise();

        void Raise()
        {
            if (!m_selectable)
                m_selectable = GetComponent<Selectable>();

            // A disabled control doesn't respond to the interaction, so it doesn't announce it either.
            if (m_selectable && (!m_selectable.IsActive() || !m_selectable.IsInteractable()))
                return;

            m_onUserInteracted?.Invoke();
        }
    }
}
