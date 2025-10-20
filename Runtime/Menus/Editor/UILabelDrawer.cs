// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(UILabel))]
    public class UILabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null) return;

            // Improve display name for MenuView fields.
            // "m_title" is the field name on MenuView; present it as "Menu Title".
            if (property.name == "m_title")
                label = new GUIContent("Menu Title", label.tooltip);

            EditorGUI.BeginProperty(position, label, property);

            float line = EditorGUIUtility.singleLineHeight;
            float pad  = EditorGUIUtility.standardVerticalSpacing;
            float y    = position.y;

            var textProp = property.FindPropertyRelative("m_text");

#if BUCK_BASICS_ENABLE_LOCALIZATION
            var useLocalizedProp = property.FindPropertyRelative("m_useLocalizedText");
            var localizedProp    = property.FindPropertyRelative("m_localizedText");

            // Row 1: toggle
            var toggleRect = new Rect(position.x, y, position.width, line);
            useLocalizedProp.boolValue = EditorGUI.ToggleLeft(toggleRect, "Use Localized Text", useLocalizedProp.boolValue);
            y += line + pad;

            // Row 2: either localized reference or plain text
            var bodyProp  = useLocalizedProp.boolValue ? localizedProp : textProp;
            var bodyH     = EditorGUI.GetPropertyHeight(bodyProp, includeChildren: true);
            var bodyRect  = new Rect(position.x, y, position.width, bodyH);
            EditorGUI.PropertyField(bodyRect, bodyProp, label, includeChildren: true);
#else
            // No localization compiled in: single row with literal text
            var bodyH    = EditorGUI.GetPropertyHeight(textProp, includeChildren: true);
            var bodyRect = new Rect(position.x, y, position.width, bodyH);
            EditorGUI.PropertyField(bodyRect, textProp, label, includeChildren: true);
#endif

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property == null) return 0f;

            float line = EditorGUIUtility.singleLineHeight;
            float pad  = EditorGUIUtility.standardVerticalSpacing;

#if BUCK_BASICS_ENABLE_LOCALIZATION
            var useLocalizedProp = property.FindPropertyRelative("m_useLocalizedText");
            var bodyProp = useLocalizedProp.boolValue
                ? property.FindPropertyRelative("m_localizedText")
                : property.FindPropertyRelative("m_text");

            return line + pad + EditorGUI.GetPropertyHeight(bodyProp, includeChildren: true);
#else
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("m_text"), includeChildren: true);
#endif
        }
    }
}
#endif
