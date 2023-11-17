//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    
    // All three of these CustomEditors behave identically and have the same names for their variables so they can just inherit from NumberVariableEditor

    [CustomEditor(typeof(IntVariable))]
    public class IntVariableEditor:NumberVariableEditor
    {

    }

    [CustomEditor(typeof(FloatVariable))]
    public class FloatVariableEditor:NumberVariableEditor
    {

    }

    [CustomEditor(typeof(DoubleVariable))]
    public class DoubleVariableEditor:NumberVariableEditor
    {

    }


    public class NumberVariableEditor : Editor
    {
        /*
        Since NumberVariables are abstract they don't have actual property drawers. 
        However, this class is used as a base class for all specific NumberVariable property drawers, such as IntVariable, FloatVariable, and DoubleVariable
        */
    
        private SerializedProperty m_clampToAMin;
        private SerializedProperty m_clampMin;
        private SerializedProperty m_clampToAMax;
        private SerializedProperty m_clampMax;

        //Values only created by children of NumberVariable, but since NumberVariable istelf is abstract this is fine
        private SerializedProperty m_debugChanges;
        private SerializedProperty DefaultValue;
        
        protected void OnEnable()
        {
            //Cache serialized properties:
            m_clampToAMin = serializedObject.FindProperty("m_clampToAMin");
            m_clampMin = serializedObject.FindProperty("m_clampMin");
            m_clampToAMax = serializedObject.FindProperty("m_clampToAMax");
            m_clampMax = serializedObject.FindProperty("m_clampMax");

            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
            DefaultValue = serializedObject.FindProperty("DefaultValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();

            
            EditorGUILayout.PropertyField(m_debugChanges);
            EditorGUILayout.PropertyField(DefaultValue);


            // Get properties

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Clamps", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_clampToAMin);

            if (m_clampToAMin.boolValue)
            {
                EditorGUILayout.PropertyField(m_clampMin);
            }

            EditorGUILayout.PropertyField(m_clampToAMax);

            if (m_clampToAMax.boolValue)
            {
                EditorGUILayout.PropertyField(m_clampMax);
            }

            
            serializedObject.ApplyModifiedProperties();
            
        }

    }
}
//#endif