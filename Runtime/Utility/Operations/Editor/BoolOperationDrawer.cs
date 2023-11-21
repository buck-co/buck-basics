#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(BoolOperation))]
    public class BoolOperationDrawer : BaseOperationDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();

            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            EditorGUILayout.PropertyField(operation);

            SerializedProperty boolA = property.FindPropertyRelative("m_boolA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (boolA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

            EditorGUILayout.PropertyField(boolA);

            GUI.backgroundColor = Color.white;//Clear tint if it got set
            
            GUIStyle style = base.CenteredLightLabel;

            EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true)); 


            if((BoolOperation.Operations)(operation.enumValueIndex) == BoolOperation.Operations.SetTo)
            {
                //Set to
                SerializedProperty boolB = property.FindPropertyRelative("m_boolB");
                EditorGUILayout.PropertyField(boolB);
            }
            else
            {
                //Toggle
                EditorGUILayout.LabelField("!(Bool A)", style, GUILayout.ExpandWidth(true)); 
            }
            
            SerializedProperty raiseEvent = property.FindPropertyRelative("m_raiseEvent");
            EditorGUILayout.PropertyField(raiseEvent);
            
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

    }
}
#endif