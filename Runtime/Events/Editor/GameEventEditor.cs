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
            DebugChangesGUI();
            
            RaiseGameEventButtonGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected void DebugChangesGUI()
        {
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_debugChanges);
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