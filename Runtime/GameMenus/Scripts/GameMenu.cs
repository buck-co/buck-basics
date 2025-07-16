// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Buck
{
    /// <summary>
    /// Main component for creating dynamic menus from BaseVariables
    /// </summary>
    public class GameMenu : MonoBehaviour
    {
        /// <summary>
        /// Defines a panel with its menu items
        /// </summary>
        [System.Serializable]
        public class PanelData
        {
            public string PanelName = "General";
            public List<MenuItemData> MenuItems = new List<MenuItemData>();
        }
        
        /// <summary>
        /// Defines a single menu item
        /// </summary>
        [System.Serializable]
        public class MenuItemData
        {
            public BaseVariable Variable;
            public string Label;
        }
        
        [Header("Menu Configuration")]
        [SerializeField] string m_menuName = "Game Menu";
        [SerializeField] List<PanelData> m_panels = new List<PanelData>();
        
        [Header("UI References")]
        [SerializeField] Transform m_tabContainer;
        [SerializeField] Transform m_panelContainer;
        [SerializeField] TextMeshProUGUI m_titleText;
        
        [Header("Builder Settings")]
        [SerializeField] GameMenuBuilder m_builder;
        
        List<GameMenuPanel> m_panelComponents = new List<GameMenuPanel>();
        List<TextMeshProUGUI> m_tabLabels = new List<TextMeshProUGUI>();
        GameMenuPanel m_currentPanel;
        int m_currentPanelIndex = 0;
        bool m_isInitialized = false;
        
        public string MenuName => m_menuName;
        public GameMenuPanel CurrentPanel => m_currentPanel;
        public List<GameMenuPanel> Panels => m_panelComponents;
        public int CurrentPanelIndex => m_currentPanelIndex;
        
        void Awake()
        {
            if (m_panels.Count > 0)
            {
                BuildMenu();
            }
        }
        
        void OnEnable()
        {
            if (m_isInitialized && m_panelComponents.Count > 0)
            {
                ShowPanel(0);
            }
        }
        
        /// <summary>
        /// Builds the menu UI from the configured menu items
        /// </summary>
        public void BuildMenu()
        {
            ClearMenu();
            
            if (m_titleText != null)
                m_titleText.text = m_menuName;
            
            // Build panels from data
            m_panelComponents = m_builder.BuildPanels(m_panelContainer, m_panels, this);
            
            // Create tab labels if multiple panels
            if (m_panelComponents.Count > 1)
            {
                CreateTabLabels();
            }
            else if (m_tabContainer != null)
            {
                m_tabContainer.gameObject.SetActive(false);
            }
            
            // Show first panel
            if (m_panelComponents.Count > 0)
            {
                ShowPanel(0);
            }
            
            m_isInitialized = true;
        }
        
        /// <summary>
        /// Dynamically adds menu items at runtime
        /// </summary>
        public void AddMenuItem(BaseVariable variable, string label, string panelName = "General")
        {
            // Find or create panel
            PanelData panel = m_panels.Find(p => p.PanelName == panelName);
            if (panel == null)
            {
                panel = new PanelData { PanelName = panelName };
                m_panels.Add(panel);
            }
            
            // Add menu item
            panel.MenuItems.Add(new MenuItemData
            {
                Variable = variable,
                Label = label
            });
            
            // Rebuild if already initialized
            if (m_isInitialized)
            {
                BuildMenu();
            }
        }
        
        /// <summary>
        /// Clears all menu items and panels
        /// </summary>
        public void ClearMenu()
        {
            foreach (var panel in m_panelComponents)
            {
                if (panel != null)
                {
                    panel.ClearMenuItems();
                    Destroy(panel.gameObject);
                }
            }
            m_panelComponents.Clear();
            m_tabLabels.Clear();
            
            // Clear tab labels
            if (m_tabContainer != null)
            {
                foreach (Transform child in m_tabContainer)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        
        /// <summary>
        /// Creates tab labels for panel navigation
        /// </summary>
        void CreateTabLabels()
        {
            if (m_tabContainer == null) return;
            
            m_tabContainer.gameObject.SetActive(true);
            
            // Clear existing tabs
            foreach (Transform child in m_tabContainer)
            {
                Destroy(child.gameObject);
            }
            
            // Create tab label for each panel
            for (int i = 0; i < m_panelComponents.Count; i++)
            {
                // Create tab label
                GameObject tabObj = new GameObject(m_panelComponents[i].PanelName + "Tab");
                tabObj.transform.SetParent(m_tabContainer, false);
                
                TextMeshProUGUI tabText = tabObj.AddComponent<TextMeshProUGUI>();
                tabText.text = m_panelComponents[i].PanelName;
                tabText.alignment = TextAlignmentOptions.Center;
                tabText.color = new Color(0.6f, 0.6f, 0.6f);
                tabText.fontSize = 18;
                
                RectTransform tabRect = tabObj.GetComponent<RectTransform>();
                tabRect.sizeDelta = new Vector2(150, 40);
                
                m_tabLabels.Add(tabText);
                m_panelComponents[i].Initialize(this);
            }
        }
        
        /// <summary>
        /// Updates tab label colors based on active panel
        /// </summary>
        void UpdateTabLabelColors()
        {
            for (int i = 0; i < m_tabLabels.Count; i++)
            {
                if (m_tabLabels[i] != null)
                {
                    m_tabLabels[i].color = (i == m_currentPanelIndex) ? Color.white : new Color(0.6f, 0.6f, 0.6f);
                }
            }
        }
        
        /// <summary>
        /// Shows a specific panel by index
        /// </summary>
        public void ShowPanel(int index)
        {
            if (index < 0 || index >= m_panelComponents.Count) return;
            
            m_currentPanelIndex = index;
            ShowPanel(m_panelComponents[index]);
        }
        
        /// <summary>
        /// Shows a specific panel
        /// </summary>
        public void ShowPanel(GameMenuPanel panel)
        {
            if (!m_panelComponents.Contains(panel)) return;
            
            // Hide all panels
            foreach (var p in m_panelComponents)
            {
                if (p != panel)
                    p.Hide();
            }
            
            // Show selected panel
            panel.Show();
            m_currentPanel = panel;
            
            // Update panel index
            m_currentPanelIndex = m_panelComponents.IndexOf(panel);
            
            // Update tab colors
            UpdateTabLabelColors();
        }
        
        /// <summary>
        /// Shows the next panel
        /// </summary>
        public void ShowNextPanel()
        {
            if (m_panelComponents.Count <= 1) return;
            
            int nextIndex = (m_currentPanelIndex + 1) % m_panelComponents.Count;
            ShowPanel(nextIndex);
        }
        
        /// <summary>
        /// Shows the previous panel
        /// </summary>
        public void ShowPreviousPanel()
        {
            if (m_panelComponents.Count <= 1) return;
            
            int prevIndex = m_currentPanelIndex - 1;
            if (prevIndex < 0) prevIndex = m_panelComponents.Count - 1;
            ShowPanel(prevIndex);
        }
        
        
        /// <summary>
        /// Call this to handle horizontal input for current panel's selected item
        /// </summary>
        public void HandleHorizontalInput(float horizontalInput)
        {
            if (m_currentPanel != null)
                m_currentPanel.HandleHorizontalInput(horizontalInput);
        }
        
        /// <summary>
        /// Call this in Update or when selection changes to update scroll position
        /// </summary>
        public void UpdateScrollPosition()
        {
            if (m_currentPanel != null)
                m_currentPanel.UpdateScrollPosition();
        }
        
        /// <summary>
        /// Shows a specific panel by number (1-9)
        /// </summary>
        public void ShowPanelByNumber(int number)
        {
            if (number > 0 && number <= m_panelComponents.Count)
                ShowPanel(number - 1);
        }
    }
}