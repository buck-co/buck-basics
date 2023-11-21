#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    
    // All three of these CustomEditors behave identically and have the same names for their variables so they can just inherit from NumberVariableEditor


    [CustomEditor(typeof(Vector4Variable)), CanEditMultipleObjects]
    public class VectorVariableEditor : BaseVariableEditor
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
        
        void OnEnable()
        {
            //Cache serialized properties:
            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
            DefaultValue = serializedObject.FindProperty("DefaultValue");
            
        }

        public override void OnInspectorGUI()
        {
            //Script field
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;


            serializedObject.UpdateIfRequiredOrScript();

            
            DefaultValue.vector4Value = EditorGUILayout.Vector4Field("Default Value", DefaultValue.vector4Value);

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