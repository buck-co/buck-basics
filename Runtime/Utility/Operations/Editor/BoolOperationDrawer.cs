//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(BoolOperation))]
    public class BoolOperationDrawer : BaseOperationDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            position.height = EditorGUIUtility.singleLineHeight;
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 2;
            
            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, operation);


            SerializedProperty boolA = property.FindPropertyRelative("m_boolA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (boolA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, boolA);

            GUI.backgroundColor = Color.white;//Clear tint if it got set
            
            GUIStyle style = base.CenteredLightLabel;

            position.y += EditorGUIUtility.singleLineHeight;
            base.DrawTextFieldGUI(position, "=", style);

            
            position.y += EditorGUIUtility.singleLineHeight;
            if((BoolOperation.Operations)(operation.enumValueIndex) == BoolOperation.Operations.SetTo)
            {
                //Set to
                SerializedProperty boolB = property.FindPropertyRelative("m_boolB");
                EditorGUI.PropertyField(position, boolB);
            }
            else
            {
                //Toggle
                base.DrawTextFieldGUI(position, "!(Bool A)", style); 
            }
            
            SerializedProperty raiseEvent = property.FindPropertyRelative("m_raiseEvent");
            
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, raiseEvent);
            

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        
        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * 6;
        }

    }
}
//#endif