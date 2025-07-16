// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buck.Samples
{
    public class DisplayPositionOnMap : MonoBehaviour
    {
        [SerializeField] RectTransform m_playerMarker;
        [SerializeField] Vector2IntVariable m_position;
        [SerializeField] Vector4Reference m_walkableExtents;
        
        public void OnRefreshPosition()
        {
            //Clamp position to walkable extents:
            
            if (m_position.Value.x < m_walkableExtents.Value.x)
                m_position.Value = new Vector2Int((int)(m_walkableExtents.Value.x), m_position.Value.y);

            if (m_position.Value.y < m_walkableExtents.Value.y)
                m_position.Value = new Vector2Int(m_position.Value.x, (int)(m_walkableExtents.Value.y));

            if (m_position.Value.x > m_walkableExtents.Value.z)
                m_position.Value = new Vector2Int((int)(m_walkableExtents.Value.z), m_position.Value.y);

            if (m_position.Value.y > m_walkableExtents.Value.w)
                m_position.Value = new Vector2Int(m_position.Value.x, (int)(m_walkableExtents.Value.w));

            m_playerMarker.anchoredPosition = m_position.ValueVector2;
        }
    }
}
