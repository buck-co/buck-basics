#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomEditor(typeof(GameEvent)), CanEditMultipleObjects]
    public class GameEventEditor : Editor
    {
        SerializedProperty m_debugChanges;

        void OnEnable()
        {
            // Cache serialized properties:
            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
        }
        
        public override void OnInspectorGUI()
        {
            // Script field
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;
            
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_debugChanges);
            RaiseGameEventButtonGUI();

            serializedObject.ApplyModifiedProperties();
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