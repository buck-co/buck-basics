using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Buck
{
    /// <summary>
    /// Base class for all menu items that represent BaseVariables in the UI
    /// </summary>
    public abstract class GameMenuItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] protected TextMeshProUGUI m_labelText;
        
        protected BaseVariable m_variable;
        protected Selectable m_selectable;
        protected GameMenuPanel m_parentPanel;
        
        public BaseVariable Variable => m_variable;
        public Selectable Selectable => m_selectable;
        
        public virtual void Initialize(BaseVariable variable, string label, GameMenuPanel parentPanel)
        {
            m_variable = variable;
            m_parentPanel = parentPanel;
            
            if (m_labelText != null)
                m_labelText.text = label;
            
            m_selectable = GetComponentInChildren<Selectable>();
            if (m_selectable == null)
                Debug.LogError($"GameMenuItem on {gameObject.name} has no Selectable component!");
                
            SetupVariableListeners();
            UpdateDisplay();
        }
        
        protected virtual void OnEnable()
        {
            if (m_variable != null)
            {
                SetupVariableListeners();
                UpdateDisplay();
            }
        }
        
        protected virtual void OnDisable()
        {
            if (m_variable != null)
                RemoveVariableListeners();
        }
        
        protected abstract void SetupVariableListeners();
        protected abstract void RemoveVariableListeners();
        protected abstract void UpdateDisplay();
        
        public virtual void OnSelect(BaseEventData eventData)
        {
            // Override in derived classes if needed
        }
        
        public virtual void OnDeselect(BaseEventData eventData)
        {
            // Override in derived classes if needed
        }
        
        /// <summary>
        /// Handle navigation input (left/right for value changes)
        /// </summary>
        public abstract void HandleHorizontalInput(float input);
    }
}