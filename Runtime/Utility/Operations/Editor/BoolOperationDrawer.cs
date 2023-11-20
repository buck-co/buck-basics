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

            label = EditorGUI.BeginProperty(position, label, property);
            
            EditorGUI.BeginChangeCheck();

            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            EditorGUILayout.PropertyField(operation);

            SerializedProperty boolA = property.FindPropertyRelative("m_boolA");
            EditorGUILayout.PropertyField(boolA);
            
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
            
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

    }
}
//#endif