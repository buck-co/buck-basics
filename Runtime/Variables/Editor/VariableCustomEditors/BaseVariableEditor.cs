#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    // All of these CustomEditors behave identically and have the same names for their serialized variables so they can just inherit from BaseVariableEditor
    
    [CustomEditor(typeof(BoolVariable)), CanEditMultipleObjects]
    public class BoolVariableEditor:BaseVariableEditor<bool>{}

    [CustomEditor(typeof(ColorVariable)), CanEditMultipleObjects]
    public class ColorVariableEditor:BaseVariableEditor<Color>{}

    [CustomEditor(typeof(GameObjectVariable)), CanEditMultipleObjects]
    public class GameObjectVariableEditor:BaseVariableEditor<GameObject>{}

    [CustomEditor(typeof(MaterialVariable)), CanEditMultipleObjects]
    public class MaterialVariableEditor:BaseVariableEditor<Material>{}

    [CustomEditor(typeof(QuaternionVariable)), CanEditMultipleObjects]
    public class QuaternionVariableEditor:BaseVariableEditor<Quaternion>{}

    [CustomEditor(typeof(SpriteVariable)), CanEditMultipleObjects]
    public class SpriteVariableEditor:BaseVariableEditor<Sprite>{}

    [CustomEditor(typeof(StringVariable)), CanEditMultipleObjects]
    public class StringVariableEditor:BaseVariableEditor<string>{}

    [CustomEditor(typeof(Texture2DVariable)), CanEditMultipleObjects]
    public class Texture2DVariableEditor:BaseVariableEditor<Texture2D>{}

    [CustomEditor(typeof(Vector2IntVariable)), CanEditMultipleObjects]
    public class Vector2IntVariableEditor:BaseVariableEditor<Vector4>{}

    [CustomEditor(typeof(Vector2Variable)), CanEditMultipleObjects]
    public class Vector2VariableEditor:BaseVariableEditor<Vector4>{}

    [CustomEditor(typeof(Vector3IntVariable)), CanEditMultipleObjects]
    public class Vector3IntVariableEditor:BaseVariableEditor<Vector4>{}

    [CustomEditor(typeof(Vector3Variable)), CanEditMultipleObjects]
    public class Vector3VariableEditor:BaseVariableEditor<Vector4>{}
    
    [CustomEditor(typeof(Vector4Variable)), CanEditMultipleObjects]
    public class Vector4VariableEditor:BaseVariableEditor<Vector4>{}

    //Vector4Variable has it's own separate editor, Vector4VariableEditor.cs

    
    public class BaseVariableEditor<T> : GameEventEditor
    {
        SerializedProperty m_debugChanges;
        SerializedProperty m_defaultValue;
        SerializedProperty m_resetOnRestart;
        SerializedProperty m_restartEvents;

        void OnEnable()
        {
            // Cache serialized properties:
            m_debugChanges = serializedObject.FindProperty("m_debugChanges");
            m_defaultValue = serializedObject.FindProperty("m_defaultValue");
            m_resetOnRestart = serializedObject.FindProperty("m_resetOnRestart");
            m_restartEvents = serializedObject.FindProperty("m_restartEvents");
        }

        public override void OnInspectorGUI()
        {
            // Script field
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;
            
            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_defaultValue);
            EditorGUILayout.PropertyField(m_resetOnRestart);
            if (m_resetOnRestart.boolValue)
                EditorGUILayout.PropertyField(m_restartEvents);
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_debugChanges);
            LogValueButtonGUI();
            base.RaiseGameEventButtonGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected void LogValueButtonGUI()
        {
            GUI.enabled = Application.isPlaying;

            BaseVariable<T> e = target as BaseVariable<T>;
            if (GUILayout.Button("Log Value"))
                e.LogValue();
        }
    }
}
#endif
