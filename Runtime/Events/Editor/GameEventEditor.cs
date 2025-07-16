// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomEditor(typeof(GameEvent)), CanEditMultipleObjects]
    public class GameEventEditor : Editor
    {
        protected SerializedProperty m_debugChanges;

        protected virtual void OnEnable()
        {
            // Cache serialized properties:
            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
        }
        
        public override void OnInspectorGUI()
        {
            ScriptFieldGUI();
            
            DebugChangesGUI();
            
            RaiseGameEventButtonGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected void ScriptFieldGUI()
        {
            // Script field
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;
            
            serializedObject.UpdateIfRequiredOrScript();
        }

        protected void DebugChangesGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_debugChanges);
            EditorGUILayout.Space();
        }

        protected void RaiseGameEventButtonGUI()
        {
            GUI.enabled = Application.isPlaying;

            GameEvent e = target as GameEvent;
            if (GUILayout.Button("Raise GameEvent"))
                e.Raise();
        }
    }
}
#endif