// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Handles building the GameMenu UI from BaseVariables
    /// </summary>
    [System.Serializable]
    public class GameMenuBuilder
    {
        [Header("Menu Item Prefabs")]
        [SerializeField] GameObject m_togglePrefab;
        [SerializeField] GameObject m_sliderPrefab;
        
        [Header("Panel Prefab")]
        [SerializeField] GameObject m_panelPrefab;
        
        [Header("Layout Settings")]
        [SerializeField] float m_itemSpacing = 50f;
        [SerializeField] int m_paddingLeft = 10;
        [SerializeField] int m_paddingRight = 10;
        [SerializeField] int m_paddingTop = 10;
        [SerializeField] int m_paddingBottom = 10;
        [SerializeField] float m_defaultItemHeight = 20f;
        
        /// <summary>
        /// Builds menu panels from a list of panel data
        /// </summary>
        public List<GameMenuPanel> BuildPanels(Transform container, List<GameMenu.PanelData> panelDataList, GameMenu parentMenu)
        {
            List<GameMenuPanel> panels = new List<GameMenuPanel>();
            
            foreach (var panelData in panelDataList)
            {
                if (panelData.MenuItems.Count == 0) continue;
                
                GameMenuPanel panel = CreatePanel(container, panelData.PanelName, parentMenu);
                
                // Add menu items to panel
                foreach (var menuItemData in panelData.MenuItems)
                {
                    GameMenuItem menuItem = CreateMenuItem(panel.MenuItemContainer, menuItemData, panel);
                    if (menuItem != null)
                        panel.AddMenuItem(menuItem);
                }
                
                panels.Add(panel);
            }
            
            return panels;
        }
        
        /// <summary>
        /// Creates a panel with the given name
        /// </summary>
        GameMenuPanel CreatePanel(Transform container, string panelName, GameMenu parentMenu)
        {
            GameObject panelObj;
            
            if (m_panelPrefab != null)
            {
                panelObj = GameObject.Instantiate(m_panelPrefab, container);
            }
            else
            {
                // Create default panel structure
                panelObj = CreateDefaultPanel(container);
            }
            
            panelObj.name = panelName + "Panel";
            
            GameMenuPanel panel = panelObj.GetComponent<GameMenuPanel>();
            if (panel == null)
                panel = panelObj.AddComponent<GameMenuPanel>();
                
            // Set panel name using the public setter
            panel.SetPanelName(panelName);
            panel.Initialize(parentMenu);
            
            return panel;
        }
        
        /// <summary>
        /// Creates a default panel structure when no prefab is provided
        /// </summary>
        GameObject CreateDefaultPanel(Transform container)
        {
            GameObject panelObj = new GameObject("Panel");
            panelObj.transform.SetParent(container, false);
            
            RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            
            // Add background
            //Image bg = panelObj.AddComponent<Image>();
            //bg.color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
            
            // Create scroll view
            GameObject scrollView = CreateScrollView(panelObj.transform);
            
            // Get or create menu item container
            Transform content = scrollView.transform.Find("Viewport/Content");
            
            // Add vertical layout group to content
            VerticalLayoutGroup layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = m_itemSpacing;
            layoutGroup.padding = new RectOffset(m_paddingLeft, m_paddingRight, m_paddingTop, m_paddingBottom);
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            
            // Add content size fitter
            ContentSizeFitter sizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
            sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            
            // Set references
            GameMenuPanel panel = panelObj.AddComponent<GameMenuPanel>();
            panel.SetMenuItemContainer(content);
            panel.SetScrollRect(scrollView.GetComponent<ScrollRect>());
            
            return panelObj;
        }
        
        /// <summary>
        /// Creates a scroll view UI element
        /// </summary>
        GameObject CreateScrollView(Transform parent)
        {
            GameObject scrollView = new GameObject("ScrollView");
            scrollView.transform.SetParent(parent, false);
            
            RectTransform scrollRect = scrollView.AddComponent<RectTransform>();
            scrollRect.anchorMin = Vector2.zero;
            scrollRect.anchorMax = Vector2.one;
            scrollRect.offsetMin = Vector2.zero;
            scrollRect.offsetMax = Vector2.zero;
            scrollRect.anchoredPosition = Vector2.zero;
            scrollRect.sizeDelta = Vector2.zero;
            
            ScrollRect scroll = scrollView.AddComponent<ScrollRect>();
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.scrollSensitivity = 30f;
            
            // Create viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(scrollView.transform, false);
            
            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.offsetMin = Vector2.zero;
            viewportRect.offsetMax = Vector2.zero;
            viewportRect.anchoredPosition = Vector2.zero;
            viewportRect.sizeDelta = Vector2.zero;
            
            viewport.AddComponent<RectMask2D>();
            
            // Create content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);
            
            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.anchoredPosition = Vector2.zero;
            contentRect.sizeDelta = new Vector2(0, 0);
            
            // Set scroll rect references
            scroll.content = contentRect;
            scroll.viewport = viewportRect;
            
            return scrollView;
        }
        
        /// <summary>
        /// Creates a menu item for the given variable
        /// </summary>
        GameMenuItem CreateMenuItem(Transform container, GameMenu.MenuItemData menuItemData, GameMenuPanel panel)
        {
            if (menuItemData.Variable == null)
            {
                Debug.LogError("Cannot create menu item with null variable");
                return null;
            }
            
            GameObject prefab = GetPrefabForVariable(menuItemData.Variable);
            if (prefab == null)
            {
                Debug.LogError($"No prefab found for variable type {menuItemData.Variable.GetType()}");
                return null;
            }
            
            GameObject itemObj = GameObject.Instantiate(prefab, container);
            itemObj.name = menuItemData.Label + "MenuItem";
            
            GameMenuItem menuItem = itemObj.GetComponent<GameMenuItem>();
            if (menuItem == null)
            {
                Debug.LogError($"Prefab {prefab.name} does not have a GameMenuItem component");
                UnityEngine.Object.Destroy(itemObj);
                return null;
            }
            
            // Initialize the menu item
            string label = string.IsNullOrEmpty(menuItemData.Label) ? menuItemData.Variable.name : menuItemData.Label;
            menuItem.Initialize(menuItemData.Variable, label, panel);
            
            // Ensure the menu item has proper layout sizing
            LayoutElement layoutElement = itemObj.GetComponent<LayoutElement>();
            if (!layoutElement)
            {
                layoutElement = itemObj.AddComponent<LayoutElement>();
            }
            layoutElement.minHeight = m_defaultItemHeight;
            //layoutElement.preferredHeight = m_defaultItemHeight;
            
            // Ensure proper RectTransform setup
            RectTransform itemRect = itemObj.GetComponent<RectTransform>();
            if (itemRect)
            {
                itemRect.anchorMin = new Vector2(0, 1);
                itemRect.anchorMax = new Vector2(1, 1);
                itemRect.pivot = new Vector2(0.5f, 0.5f);
                itemRect.sizeDelta = new Vector2(0, m_defaultItemHeight);
            }
            
            return menuItem;
        }
        
        /// <summary>
        /// Gets the appropriate prefab for a variable type
        /// </summary>
        GameObject GetPrefabForVariable(BaseVariable variable)
        {
            if (variable is BoolVariable)
                return m_togglePrefab;
                
            if (variable is NumberVariable)
                return m_sliderPrefab;
                
            return null;
        }
    }
}