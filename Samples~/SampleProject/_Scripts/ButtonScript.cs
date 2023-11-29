using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Buck.Samples
{
    public class ButtonScript : MonoBehaviour
    {

        Button m_button;
        [SerializeField] Condition[] m_conditions;
        [SerializeField] BoolOperation[] m_boolOperations;
        [SerializeField] NumberOperation[] m_numberOperations;
        [SerializeField] VectorOperation[] m_vectorOperations;
        
        void Awake()
        {
            m_button = GetComponent<Button>();
            OnRefreshButtonAvailable();
        }
        
        public void OnButtonPressed()
        {
            //Double check conditions are passed (technically should be impossible since button shoudln't be interactable if they aren't)
            if (m_conditions.PassConditions())
            {
                //Execute each collection of operations
                m_boolOperations.Execute();
                m_numberOperations.Execute();
                m_vectorOperations.Execute();
            }
        }
        // Update is called once per frame
        public void OnRefreshButtonAvailable()
        {
            //Set whether the button is interactable based on if it's conditions are passed or not
            m_button.interactable = m_conditions.PassConditions();
        }
    }
}
