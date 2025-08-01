// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Buck
{
#if USE_LOCALIZATION
    [CustomPropertyDrawer(typeof(StringReference))]
    public class StringReferenceDrawer : BaseReferenceDrawer
    {
        protected override string[] GetPopupOptions() => new[] { "Use Constant", "Use Variable", "Use LocalizedString" };

        protected override int GetCurrentSelection(SerializedProperty property)
        {
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            SerializedProperty useLocalizedString = property.FindPropertyRelative("UseLocalizedString");
            
            if (useVariable.boolValue) return 1;
            if (useLocalizedString != null && useLocalizedString.boolValue) return 2;
            return 0;
        }

        protected override void HandleSelectionChange(SerializedProperty property, int newSelection)
        {
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            SerializedProperty useLocalizedString = property.FindPropertyRelative("UseLocalizedString");
            SerializedProperty variable = property.FindPropertyRelative("Variable");
            SerializedProperty localizedString = property.FindPropertyRelative("LocalizedString");

            // Clear all flags first
            useVariable.boolValue = false;
            if (useLocalizedString != null) useLocalizedString.boolValue = false;

            // Clear unused references
            switch (newSelection)
            {
                case 0: // Constant
                    variable.objectReferenceValue = null;
                    // Don't try to set objectReferenceValue on LocalizedString
                    // LocalizedString will keep its value but won't be used
                    break;
                case 1: // Variable
                    useVariable.boolValue = true;
                    // Don't try to set objectReferenceValue on LocalizedString
                    break;
                case 2: // LocalizedString
                    if (useLocalizedString != null) useLocalizedString.boolValue = true;
                    variable.objectReferenceValue = null;
                    break;
            }
        }

        protected override void CreateValueGUI(Rect position, SerializedProperty property, int currentSelection)
        {
            switch (currentSelection)
            {
                case 0: // Constant
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("ConstantValue"), GUIContent.none);
                    break;
                case 1: // Variable
                    var variable = property.FindPropertyRelative("Variable");
                    if (variable.objectReferenceValue == null)
                        GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);
                    EditorGUI.PropertyField(position, variable, GUIContent.none);
                    GUI.backgroundColor = Color.white;
                    break;
                case 2: // LocalizedString
                    var localizedString = property.FindPropertyRelative("LocalizedString");
                    if (localizedString != null)
                    {
                        // LocalizedString is not an object reference, it's a complex serialized type
                        // Just draw it without checking for null
                        EditorGUI.PropertyField(position, localizedString, GUIContent.none);
                    }
                    break;
            }
        }
        
        protected override bool ShouldDrawCustomField(int currentSelection)
        {
            // Use custom drawing for LocalizedString
            return currentSelection == 2;
        }
        
        protected override void DrawCustomField(Rect position, SerializedProperty property, int currentSelection)
        {
            if (currentSelection == 2) // LocalizedString
            {
                var localizedString = property.FindPropertyRelative("LocalizedString");
                if (localizedString != null)
                {
                    // Calculate button area
                    var popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                    popupStyle.imagePosition = ImagePosition.ImageOnly;
                    
                    Rect buttonRect = new Rect(position);
                    buttonRect.yMin += popupStyle.margin.top;
                    buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
                    buttonRect.x = position.x + EditorGUIUtility.labelWidth - buttonRect.width;
                    
                    // Draw the popup button
                    EditorGUI.BeginChangeCheck();
                    int result = EditorGUI.Popup(buttonRect, currentSelection, GetPopupOptions(), popupStyle);
                    if (result != currentSelection)
                    {
                        HandleSelectionChange(property, result);
                    }
                    if (EditorGUI.EndChangeCheck())
                        property.serializedObject.ApplyModifiedProperties();
                    
                    // Draw the LocalizedString field with full width
                    // The LocalizedString drawer will handle its own label internally
                    EditorGUI.PropertyField(position, localizedString, GUIContent.none, true);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int currentSelection = GetCurrentSelection(property);
            
            if (currentSelection == 2) // LocalizedString
            {
                var localizedString = property.FindPropertyRelative("LocalizedString");
                if (localizedString != null)
                {
                    // LocalizedString can be quite tall when expanded
                    return EditorGUI.GetPropertyHeight(localizedString, GUIContent.none, true);
                }
            }
            
            return base.GetPropertyHeight(property, label);
        }
    }
    
#else
    
    [CustomPropertyDrawer(typeof(StringReference))]
    public class StringReferenceDrawer : BaseReferenceDrawer { }
    
#endif
}
#endif