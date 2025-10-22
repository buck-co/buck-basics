// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Buck
{
    [DisallowMultipleComponent]
    [AddComponentMenu("BUCK/UI/TMP Dropdown Ensure Visible")]
    public class TMPDropdownEnsureVisible : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown m_dropdown;
        [SerializeField] ScrollRect   m_scrollRect;

        RectTransform m_viewport;
        RectTransform m_content;
        GameObject    m_lastSelected;

        void Awake()
        {
            if (!m_scrollRect) m_scrollRect = GetComponent<ScrollRect>();
            if (!m_scrollRect) return;

            m_viewport = m_scrollRect.viewport ? m_scrollRect.viewport : m_scrollRect.GetComponent<RectTransform>();
            m_content  = m_scrollRect.content;
        }

        void LateUpdate()
        {
            if (!m_scrollRect || !m_content || !m_viewport) return;
            if (m_dropdown && !m_dropdown.IsExpanded) return;

            var es = EventSystem.current;
            var sel = es ? es.currentSelectedGameObject : null;
            if (!sel) return;

            var selRT = sel.GetComponent<RectTransform>();
            if (!selRT || !selRT.IsChildOf(m_content)) return;

            if (m_lastSelected == sel) return;
            m_lastSelected = sel;

            EnsureVisible(selRT);
        }

        void EnsureVisible(RectTransform target)
        {
            if (!target || !m_viewport) return;

            var viewCorners  = new Vector3[4];
            var itemCorners  = new Vector3[4];

            m_viewport.GetWorldCorners(viewCorners);
            target.GetWorldCorners(itemCorners);

            float viewTop    = viewCorners[1].y; // top-left
            float viewBottom = viewCorners[0].y; // bottom-left
            float itemTop    = itemCorners[1].y;
            float itemBottom = itemCorners[0].y;

            float dy = 0f;
            if (itemTop > viewTop)         dy = viewTop - itemTop;       // scroll up
            else if (itemBottom < viewBottom) dy = viewBottom - itemBottom; // scroll down

            if (Mathf.Abs(dy) > 0.01f)
            {
                var pos = m_content.position;
                pos.y += dy;
                m_content.position = pos;
            }
        }

        public void Bind(TMP_Dropdown dropdown, ScrollRect sr)
        {
            m_dropdown   = dropdown;
            m_scrollRect = sr;
            m_viewport   = m_scrollRect.viewport ? m_scrollRect.viewport : m_scrollRect.GetComponent<RectTransform>();
            m_content    = m_scrollRect.content;
        }
    }
}
