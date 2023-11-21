#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    
    // All three of these CustomEditors behave identically and have the same names for their serialized variables so they can just inherit from BaseVariableEditor
    
    [CustomEditor(typeof(BoolVariable)), CanEditMultipleObjects]
    public class BoolVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(ColorVariable)), CanEditMultipleObjects]
    public class ColorVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(GameObjectVariable)), CanEditMultipleObjects]
    public class GameObjectVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(MaterialVariable)), CanEditMultipleObjects]
    public class MaterialVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(QuaternionVariable)), CanEditMultipleObjects]
    public class QuaternionVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(SpriteVariable)), CanEditMultipleObjects]
    public class SpriteVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(StringVariable)), CanEditMultipleObjects]
    public class StringVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(Texture2DVariable)), CanEditMultipleObjects]
    public class Texture2DVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(Vector2IntVariable)), CanEditMultipleObjects]
    public class Vector2IntVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(Vector2Variable)), CanEditMultipleObjects]
    public class Vector2VariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(Vector3IntVariable)), CanEditMultipleObjects]
    public class Vector3IntVariableEditor:BaseVariableEditor{}

    [CustomEditor(typeof(Vector3Variable)), CanEditMultipleObjects]
    public class Vector3VariableEditor:BaseVariableEditor{}

    //Vector4Variable has it's own separate editor, Vector4VariableEditor.cs

    
    public class BaseVariableEditor : GameEventEditor
    {
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

            EditorGUILayout.PropertyField(DefaultValue);
            
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

            BaseVariable e = target as BaseVariable;
            if (GUILayout.Button("Log Value"))
                e.LogValue();
        }

    }
}
#endif