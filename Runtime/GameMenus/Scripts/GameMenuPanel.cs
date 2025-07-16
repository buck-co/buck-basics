// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Buck
{
    /// <summary>
    /// Represents a panel within a GameMenu that contains multiple menu items
    /// </summary>
    public class GameMenuPanel : MonoBehaviour
    {
        string m_panelName = "Panel";
        Transform m_menuItemContainer;
        ScrollRect m_scrollRect;
        
        List<GameMenuItem> m_menuItems = new List<GameMenuItem>();
        GameMenu m_parentMenu;
        
        public string PanelName => m_panelName;
        public List<GameMenuItem> MenuItems => m_menuItems;
        public Transform MenuItemContainer => m_menuItemContainer;
        
        public void SetPanelName(string name)
        {
            m_panelName = name;
        }
        
        public void SetMenuItemContainer(Transform container)
        {
            m_menuItemContainer = container;
        }
        
        public void SetScrollRect(ScrollRect scrollRect)
        {
            m_scrollRect = scrollRect;
        }
        
        public void Initialize(GameMenu parentMenu)
        {
            m_parentMenu = parentMenu;
        }
        
        public void AddMenuItem(GameMenuItem menuItem)
        {
            m_menuItems.Add(menuItem);
            
            // Set up navigation
            UpdateNavigation();
        }
        
        public void ClearMenuItems()
        {
            foreach (var item in m_menuItems)
            {
                if (item != null)
                    Destroy(item.gameObject);
            }
            m_menuItems.Clear();
        }
        
        void UpdateNavigation()
        {
            for (int i = 0; i < m_menuItems.Count; i++)
            {
                if (m_menuItems[i] == null || m_menuItems[i].Selectable == null)
                    continue;
                
                Navigation nav = new Navigation
                {
                    mode = Navigation.Mode.Explicit
                };
                
                // Vertical navigation
                if (i > 0 && m_menuItems[i - 1] != null && m_menuItems[i - 1].Selectable != null)
                    nav.selectOnUp = m_menuItems[i - 1].Selectable;
                    
                if (i < m_menuItems.Count - 1 && m_menuItems[i + 1] != null && m_menuItems[i + 1].Selectable != null)
                    nav.selectOnDown = m_menuItems[i + 1].Selectable;
                
                m_menuItems[i].Selectable.navigation = nav;
            }
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            
            // Select first item if available
            if (m_menuItems.Count > 0 && m_menuItems[0] != null && m_menuItems[0].Selectable != null)
            {
                EventSystem.current.SetSelectedGameObject(m_menuItems[0].Selectable.gameObject);
            }
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }
        
        /// <summary>
        /// Call this method to handle horizontal input for the currently selected menu item
        /// </summary>
        public void HandleHorizontalInput(float horizontalInput)
        {
            if (!gameObject.activeSelf) return;
            
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                GameMenuItem menuItem = selected.GetComponentInParent<GameMenuItem>();
                if (menuItem != null && m_menuItems.Contains(menuItem))
                {
                    menuItem.HandleHorizontalInput(horizontalInput);
                }
            }
        }
        
        /// <summary>
        /// Updates the scroll position to keep the selected item visible
        /// Call this in Update or when selection changes
        /// </summary>
        public void UpdateScrollPosition()
        {
            if (m_scrollRect == null) return;
            
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            if (selected != null)
            {
                RectTransform selectedRect = selected.GetComponent<RectTransform>();
                if (selectedRect != null && selectedRect.IsChildOf(m_scrollRect.content))
                {
                    // Calculate position to center the selected item
                    float selectedY = -selectedRect.anchoredPosition.y;
                    float contentHeight = m_scrollRect.content.rect.height;
                    float viewportHeight = m_scrollRect.viewport.rect.height;
                    
                    if (contentHeight > viewportHeight)
                    {
                        float normalizedPos = Mathf.Clamp01((selectedY - viewportHeight * 0.5f) / (contentHeight - viewportHeight));
                        m_scrollRect.verticalNormalizedPosition = 1f - normalizedPos;
                    }
                }
            }
        }
    }
}