using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Buck.Samples
{
public class DisplayNumberVariable : MonoBehaviour
{
    TextMeshProUGUI m_textMeshProUGUI;

    [SerializeField] NumberVariable m_numberVariable;
    
    void Awake()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        RefreshText();
    }
    
    void RefreshText()
    {
        m_textMeshProUGUI.text = m_numberVariable.name + " = " + m_numberVariable;
    }
    
    public void OnVariableRefreshed()
    {
        RefreshText();
    }
}
}
