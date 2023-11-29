using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Buck.Samples
{
public class DisplayNumberVariable : MonoBehaviour
{
    TextMeshProUGUI m_textMeshProUGUI;

    [SerializeField] BaseVariable m_baseVariable;
    
    void Awake()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        RefreshText();
    }
    
    void RefreshText()
    {
        m_textMeshProUGUI.text = m_baseVariable.name + " = " + m_baseVariable.ValueAsString;
    }
    // Update is called once per frame
    public void OnVariableRefreshed()
    {
        RefreshText();
    }
}
}
