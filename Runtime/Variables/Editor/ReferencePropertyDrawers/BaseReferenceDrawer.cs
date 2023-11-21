#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    //All of these VariableReference types use the BaseReferenceDrawer as their parent so that they can share the same base code.
    //The only two variables that have their own custom editor are VectorReference and Vector4Reference since Vector4s don't GUI in a single line out of the box
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(ColorReference))]
    public class ColorReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(DoubleReference))]
    public class DoubleReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(FloatReference))]
    public class FloatReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(GameObjectReference))]
    public class GameObjectReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(MaterialReference))]
    public class MaterialReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(NumberReference))]
    public class NumberReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(QuaternionReference))]
    public class QuaternionReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(SpriteReference))]
    public class SpriteReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(StringReference))]
    public class StringReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(Texture2DReference))]
    public class Texture2DReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(Vector2IntReference))]
    public class Vector2IntReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(Vector2Reference))]
    public class Vector2ReferenceDrawer:BaseReferenceDrawer{}

    [CustomPropertyDrawer(typeof(Vector3IntReference))]
    public class Vector3IntReferenceDrawer:BaseReferenceDrawer{}
    
    [CustomPropertyDrawer(typeof(Vector3Reference))]
    public class Vector3ReferenceDrawer:BaseReferenceDrawer{}


    public class BaseReferenceDrawer : PropertyDrawer
    {
        /// <summary>
        /// Options to display in the popup to select constant or variable.
        /// </summary>
        private readonly string[] popupOptions = 
            { "Use Constant", "Use Variable" };

        /// <summary> Cached style to use to draw the popup button. </summary>
        private GUIStyle popupStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (popupStyle == null)
            {
                popupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"));
                popupStyle.imagePosition = ImagePosition.ImageOnly;
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            
            EditorGUI.BeginChangeCheck();

            // Get properties
            SerializedProperty useVariable = property.FindPropertyRelative("UseVariable");
            SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
            SerializedProperty variable = property.FindPropertyRelative("Variable");

            // Calculate rect for configuration button
            Rect buttonRect = new Rect(position);
            buttonRect.yMin += popupStyle.margin.top;
            buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
            position.xMin = buttonRect.xMax;

            // Store old indent level and set it to 0, the PrefixLabel takes care of it
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useVariable.boolValue ? 1 : 0, popupOptions, popupStyle);

            useVariable.boolValue = result == 1;

            CreateValueGUI(position, useVariable, constantValue, variable);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        protected virtual void CreateValueGUI(Rect position, SerializedProperty useVariable, SerializedProperty constantValue, SerializedProperty variable)
        {
            if (useVariable.boolValue)
            {
                //Tint field light red if this Reference is using the variable but the variable is null (likely to create errors)
                if (variable.objectReferenceValue == null)
                    GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);
                
                EditorGUI.PropertyField(position, variable, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(position, constantValue, GUIContent.none);
            }
        }
    }
}
#endif