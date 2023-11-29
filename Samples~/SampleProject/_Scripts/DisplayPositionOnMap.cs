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

        IntVariable m_intVariable;

        
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

            m_intVariable.Value = 4;

            if (m_intVariable.Value > 0)
            {
                m_intVariable.Value -= 10;
                m_intVariable.Raise();//Notify any listeners to the IntVariable event that it has changed
            }
        }
    }
}
