// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

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
    public class Vector2IntVariableEditor:BaseVariableEditor<Vector4>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.vector4Value = (Vector2)EditorGUILayout.Vector2IntField("Default Value", ((Vector2)m_defaultValue.vector4Value).ToVector2Int());
    }

    [CustomEditor(typeof(Vector2Variable)), CanEditMultipleObjects]
    public class Vector2VariableEditor:BaseVariableEditor<Vector4>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.vector4Value = EditorGUILayout.Vector2Field("Default Value", (Vector2)m_defaultValue.vector4Value);
    }

    [CustomEditor(typeof(Vector3IntVariable)), CanEditMultipleObjects]
    public class Vector3IntVariableEditor : BaseVariableEditor<Vector4>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.vector4Value = (Vector3)EditorGUILayout.Vector3IntField("Default Value", ((Vector3)m_defaultValue.vector4Value).ToVector3Int());
    }

    [CustomEditor(typeof(Vector3Variable)), CanEditMultipleObjects]
    public class Vector3VariableEditor : BaseVariableEditor<Vector4>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.vector4Value = EditorGUILayout.Vector3Field("Default Value", (Vector3)m_defaultValue.vector4Value);
    }

    [CustomEditor(typeof(Vector4Variable)), CanEditMultipleObjects]
    public class Vector4VariableEditor : BaseVariableEditor<Vector4>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.vector4Value = EditorGUILayout.Vector4Field("Default Value", m_defaultValue.vector4Value);
    }

    public class BaseVariableEditor<T> : GameEventEditor
    {
        protected SerializedProperty m_defaultValue;
        protected SerializedProperty m_labelText;
#if BUCK_BASICS_ENABLE_LOCALIZATION
        protected SerializedProperty m_localizeLabelText;
        protected SerializedProperty m_localizedLabelText;
#endif
        protected SerializedProperty m_resetOnRestart;
        protected SerializedProperty m_restartEvents;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Cache serialized properties:
            m_defaultValue = serializedObject.FindProperty("m_defaultValue");
            m_labelText = serializedObject.FindProperty("m_labelText");
#if BUCK_BASICS_ENABLE_LOCALIZATION
            m_localizeLabelText = serializedObject.FindProperty("m_localizeLabelText");
            m_localizedLabelText = serializedObject.FindProperty("m_localizedLabelText");
#endif
            m_resetOnRestart = serializedObject.FindProperty("m_resetOnRestart");
            m_restartEvents = serializedObject.FindProperty("m_restartEvents");
        }

        public override void OnInspectorGUI()
        {
            ScriptFieldGUI();
            DefaultValueGUI();
            LabelGUI();
            
            EditorGUILayout.PropertyField(m_resetOnRestart);
            if (m_resetOnRestart.boolValue)
                EditorGUILayout.PropertyField(m_restartEvents);
            
            DebugChangesGUI();

            LogValueButtonGUI();
            RaiseGameEventButtonGUI();

            serializedObject.ApplyModifiedProperties();
        }
        
        protected virtual void LabelGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("UI Label Text", EditorStyles.boldLabel);
#if BUCK_BASICS_ENABLE_LOCALIZATION
            EditorGUILayout.PropertyField(m_localizeLabelText);

            EditorGUILayout.PropertyField(m_localizeLabelText.boolValue ? m_localizedLabelText : m_labelText);
#else
            EditorGUILayout.PropertyField(m_labelText);
#endif
            EditorGUILayout.Space();
        }

        protected virtual void LogValueButtonGUI()
        {
            GUI.enabled = Application.isPlaying;

            BaseVariable<T> e = target as BaseVariable<T>;
            if (GUILayout.Button("Log Value"))
                e.LogValue();
        }
        
        protected virtual void DefaultValueGUI()
            => EditorGUILayout.PropertyField(m_defaultValue);
    }
}
#endif
