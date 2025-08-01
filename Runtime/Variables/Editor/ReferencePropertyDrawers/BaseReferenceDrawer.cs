// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    // All of these VariableReference types use the BaseReferenceDrawer as their parent so that they can share the same base code.
    // Variables that have their own custom editor are StringReference and VectorReference.
    // Vector4Reference also has a custom drawer since Vector4s don't have a single-line GUI by default.
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(ColorReference))]
    public class ColorReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(DoubleReference))]
    public class DoubleReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(GameObjectReference))]
    public class GameObjectReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(MaterialReference))]
    public class MaterialReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(NumberReference))]
    public class NumberReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(QuaternionReference))]
    public class QuaternionReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(SpriteReference))]
    public class SpriteReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(Texture2DReference))]
    public class Texture2DReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(Vector2IntReference))]
    public class Vector2IntReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(Vector2Reference))]
    public class Vector2ReferenceDrawer : BaseReferenceDrawer { }

    [CustomPropertyDrawer(typeof(Vector3IntReference))]
    public class Vector3IntReferenceDrawer : BaseReferenceDrawer { }
    
    [CustomPropertyDrawer(typeof(Vector3Reference))]
    public class Vector3ReferenceDrawer : BaseReferenceDrawer { }


    public class BaseReferenceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Options to display in the popup to select constant or variable.
        /// </summary>
        protected virtual string[] GetPopupOptions() => new[] { "Use Constant", "Use Variable" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
    
            // Check if we need custom drawing
            int currentSelection = GetCurrentSelection(property);
            if (ShouldDrawCustomField(currentSelection))
            {
                // Draw the label but let the derived class handle the full field
                EditorGUI.PrefixLabel(position, label);
                DrawCustomField(position, property, currentSelection);
            }
            else
            {
                // Standard behavior
                position = EditorGUI.PrefixLabel(position, label);
        
                EditorGUI.BeginChangeCheck();

                // Calculate rect for configuration button
                Rect buttonRect = new Rect(position);
                buttonRect.yMin += popupStyle.margin.top;
                buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
                position.xMin = buttonRect.xMax;

                // Store old indent level and set it to 0, the PrefixLabel takes care of it
                int indent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                int result = EditorGUI.Popup(buttonRect, currentSelection, GetPopupOptions(), popupStyle);

                if (result != currentSelection)
                {
                    HandleSelectionChange(property, result);
                }

                CreateValueGUI(position, property, result);

                if (EditorGUI.EndChangeCheck())
                    property.serializedObject.ApplyModifiedProperties();

                EditorGUI.indentLevel = indent;
            }
    
            EditorGUI.EndProperty();
        }

        protected virtual int GetCurrentSelection(SerializedProperty property)
        {
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            return useVariable.boolValue ? 1 : 0;
        }

        protected virtual void HandleSelectionChange(SerializedProperty property, int newSelection)
        {
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

            if (newSelection == 0 && useVariable.boolValue)
            {
                variable.objectReferenceValue = null;
            }

            useVariable.boolValue = newSelection == 1;
        }

        protected virtual void CreateValueGUI(Rect position, SerializedProperty property, int currentSelection)
        {
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

            if (useVariable.boolValue)
            {
                if (variable.objectReferenceValue == null)
                    GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);
                
                EditorGUI.PropertyField(position, variable, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(position, constantValue, GUIContent.none);
            }
        }
        
        protected virtual bool ShouldDrawCustomField(int currentSelection)
        {
            return false;
        }
        
        protected virtual void DrawCustomField(Rect position, SerializedProperty property, int currentSelection)
        {
            // To be overridden
        }
    }
}
#endif
