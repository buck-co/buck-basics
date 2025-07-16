// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    // All of these CustomEditors behave identically and have the same names for their variables so they can just inherit from NumberVariableEditor

    [CustomEditor(typeof(IntVariable)), CanEditMultipleObjects]
    public class IntVariableEditor:NumberVariableEditor<int>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.doubleValue = EditorGUILayout.IntField("Default Value", (int)m_defaultValue.doubleValue);
    }

    [CustomEditor(typeof(FloatVariable)), CanEditMultipleObjects]
    public class FloatVariableEditor:NumberVariableEditor<float>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.doubleValue = EditorGUILayout.FloatField("Default Value", (float)m_defaultValue.doubleValue);
    }

    [CustomEditor(typeof(DoubleVariable)), CanEditMultipleObjects]
    public class DoubleVariableEditor:NumberVariableEditor<double>
    {
        protected override void DefaultValueGUI()
            => m_defaultValue.doubleValue = EditorGUILayout.DoubleField("Default Value", m_defaultValue.doubleValue);
    }

    public class NumberVariableEditor<T> : BaseVariableEditor<T>
    {
        /*
        Since NumberVariables are abstract they don't have actual property drawers. 
        However, this class is used as a base class for all specific NumberVariable property drawers, such as IntVariable, FloatVariable, and DoubleVariable
        */
    
        SerializedProperty m_clampToAMin;
        SerializedProperty m_clampMin;
        SerializedProperty m_clampToAMax;
        SerializedProperty m_clampMax;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            // Cache serialized properties:
            m_clampToAMin = serializedObject.FindProperty("m_clampToAMin");
            m_clampMin = serializedObject.FindProperty("m_clampMin");
            m_clampToAMax = serializedObject.FindProperty("m_clampToAMax");
            m_clampMax = serializedObject.FindProperty("m_clampMax");
        }

        public override void OnInspectorGUI()
        {
            ScriptFieldGUI();
            
            DefaultValueGUI();
            EditorGUILayout.PropertyField(m_resetOnRestart);
            if (m_resetOnRestart.boolValue)
                EditorGUILayout.PropertyField(m_restartEvents);
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Clamps", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_clampToAMin);

            if (m_clampToAMin.boolValue)
                EditorGUILayout.PropertyField(m_clampMin);

            EditorGUILayout.PropertyField(m_clampToAMax);

            if (m_clampToAMax.boolValue)
                EditorGUILayout.PropertyField(m_clampMax);

            DebugChangesGUI();
            
            LogValueButtonGUI();
            RaiseGameEventButtonGUI();
            
            serializedObject.ApplyModifiedProperties();
        }
        
        protected override void LogValueButtonGUI()
        {
            GUI.enabled = Application.isPlaying;

            NumberVariable e = target as NumberVariable;
            if (GUILayout.Button("Log Value"))
                e.LogValue();
        }
    }
}
#endif
