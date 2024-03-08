#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    // All of these CustomEditors behave identically and have the same names for their variables so they can just inherit from NumberVariableEditor

    [CustomEditor(typeof(IntVariable)), CanEditMultipleObjects]
    public class IntVariableEditor:NumberVariableEditor
    {

    }

    [CustomEditor(typeof(FloatVariable)), CanEditMultipleObjects]
    public class FloatVariableEditor:NumberVariableEditor
    {

    }

    [CustomEditor(typeof(DoubleVariable)), CanEditMultipleObjects]
    public class DoubleVariableEditor:NumberVariableEditor
    {

    }

    public class NumberVariableEditor : BaseVariableEditor
    {
        /*
        Since NumberVariables are abstract they don't have actual property drawers. 
        However, this class is used as a base class for all specific NumberVariable property drawers, such as IntVariable, FloatVariable, and DoubleVariable
        */
    
        SerializedProperty m_clampToAMin;
        SerializedProperty m_clampMin;
        SerializedProperty m_clampToAMax;
        SerializedProperty m_clampMax;

        // Values only created by children of NumberVariable, but since NumberVariable itself is abstract this is fine
        SerializedProperty m_debugChanges;
        SerializedProperty DefaultValue;
        
        void OnEnable()
        {
            // Cache serialized properties:
            m_clampToAMin = serializedObject.FindProperty("m_clampToAMin");
            m_clampMin = serializedObject.FindProperty("m_clampMin");
            m_clampToAMax = serializedObject.FindProperty("m_clampToAMax");
            m_clampMax = serializedObject.FindProperty("m_clampMax");

            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
            DefaultValue = serializedObject.FindProperty("DefaultValue");
        }

        public override void OnInspectorGUI()
        {
            // Script field
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;

            serializedObject.UpdateIfRequiredOrScript();
            
            EditorGUILayout.PropertyField(DefaultValue);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Clamps", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_clampToAMin);

            if (m_clampToAMin.boolValue)
                EditorGUILayout.PropertyField(m_clampMin);

            EditorGUILayout.PropertyField(m_clampToAMax);

            if (m_clampToAMax.boolValue)
                EditorGUILayout.PropertyField(m_clampMax);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            
            EditorGUILayout.PropertyField(m_debugChanges);
            base.RaiseGameEventButtonGUI();
            base.LogValueButtonGUI();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
