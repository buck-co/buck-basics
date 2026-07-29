// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomEditor(typeof(AspectRatioPolicy)), CanEditMultipleObjects]
    public class AspectRatioPolicyEditor : Editor
    {
        // Named ratios shown next to the bounds so a designer doesn't have to recognize 1.7778 on sight.
        static readonly (float Ratio, string Name)[] s_commonRatios =
        {
            (5f  / 4f,  "5:4"),
            (4f  / 3f,  "4:3"),
            (3f  / 2f,  "3:2"),
            (16f / 10f, "16:10"),
            (5f  / 3f,  "5:3"),
            (16f / 9f,  "16:9"),
            (2f  / 1f,  "2:1"),
            (64f / 27f, "21:9"),
            (32f / 9f,  "32:9")
        };

        // Relative, so it stays meaningful across the range. Wide enough that all of 2.333, 2.370 and
        // 2.389 read as "21:9", narrow enough that 4:3 and 5:4 never claim each other's name.
        const float k_nameTolerance = 0.02f;

        protected SerializedProperty m_mode;
        protected SerializedProperty m_minAspect;
        protected SerializedProperty m_maxAspect;

        protected virtual void OnEnable()
        {
            // Cache serialized properties:
            m_mode = serializedObject.FindProperty("m_mode");
            m_minAspect = serializedObject.FindProperty("m_minAspect");
            m_maxAspect = serializedObject.FindProperty("m_maxAspect");
        }

        public override void OnInspectorGUI()
        {
            ScriptFieldGUI();

            BoundsGUI();

            ValidationGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected void ScriptFieldGUI()
        {
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject((ScriptableObject)target), typeof(ScriptableObject), false);
            GUI.enabled = true;

            serializedObject.UpdateIfRequiredOrScript();
        }

        protected void BoundsGUI()
        {
            EditorGUILayout.PropertyField(m_mode);

            var mode = (AspectRatioPolicy.Modes)m_mode.enumValueIndex;

            if (mode == AspectRatioPolicy.Modes.Minimum || mode == AspectRatioPolicy.Modes.Range)
                AspectFieldGUI(m_minAspect, "Min Aspect");

            if (mode == AspectRatioPolicy.Modes.Maximum || mode == AspectRatioPolicy.Modes.Range)
                AspectFieldGUI(m_maxAspect, "Max Aspect");
        }

        protected void ValidationGUI()
        {
            var mode = (AspectRatioPolicy.Modes)m_mode.enumValueIndex;

            if (mode == AspectRatioPolicy.Modes.Off)
            {
                EditorGUILayout.HelpBox("This policy allows every aspect ratio, which is the same as not " +
                                        "assigning it at all.", MessageType.Info);
                return;
            }

            if (mode == AspectRatioPolicy.Modes.Range && m_minAspect.floatValue > m_maxAspect.floatValue)
                EditorGUILayout.HelpBox("Min Aspect is greater than Max Aspect, so nothing can pass. " +
                                        "Components using this policy will ignore it and log a warning.",
                    MessageType.Warning);
        }

        void AspectFieldGUI(SerializedProperty property, string label)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(property, new GUIContent(label, property.tooltip));
            EditorGUILayout.LabelField(GetRatioName(property.floatValue), EditorStyles.miniLabel, GUILayout.Width(52f));
            EditorGUILayout.EndHorizontal();
        }

        // Returns a parenthesized name for a ratio, or an empty string when it doesn't match a common one.
        static string GetRatioName(float ratio)
        {
            if (ratio <= 0f) return string.Empty;

            foreach (var common in s_commonRatios)
                if (Mathf.Abs(ratio - common.Ratio) <= k_nameTolerance * common.Ratio)
                    return $"({common.Name})";

            return string.Empty;
        }
    }
}
#endif
