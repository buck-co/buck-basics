// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Attach to a UI element to a variable.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Selectable))]
    [AddComponentMenu("BUCK/UI/Variable Binding")]
    public class VariableBinding : MonoBehaviour
    {
        [Tooltip("The variable to bind to this UI element.")]
        [SerializeField] BaseVariable m_variable;
        public BaseVariable Variable => m_variable;
        
        [Tooltip("Should the variable's GameEvent be raised when the UI element changes it?")]
        [SerializeField]
        bool m_raiseGameEventOnChange = true;

        public bool RaiseGameEventOnChange => m_raiseGameEventOnChange;

        Selectable m_selectable;

        public Selectable Selectable
        {
            get
            {
                if (!m_selectable)
                    m_selectable = GetComponent<Selectable>();
                return m_selectable;
            }
        }
    }
}