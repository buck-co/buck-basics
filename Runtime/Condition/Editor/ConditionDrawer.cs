#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(Condition))]
    public class ConditionDrawer : PropertyDrawer
    {


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            position.height = EditorGUIUtility.singleLineHeight;
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();


            EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 2;
            

            SerializedProperty variableType = property.FindPropertyRelative("m_variableType");
            
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, variableType);
            

            SerializedProperty var_A = null;
            SerializedProperty var_B = null;

            switch ((Condition.VariableType)(variableType.enumValueIndex))
            {
                case Condition.VariableType.Bool:
                   var_A = property.FindPropertyRelative("m_boolA");
                   var_B = property.FindPropertyRelative("m_boolB");
                break;

                case Condition.VariableType.Number:
                   var_A = property.FindPropertyRelative("m_numberA");
                   var_B = property.FindPropertyRelative("m_numberB");
                break;

                case Condition.VariableType.Vector:
                   var_A = property.FindPropertyRelative("m_vectorA");
                   var_B = property.FindPropertyRelative("m_vectorB");
                break;



                default:
                break;
            }

            position.y += EditorGUIUtility.singleLineHeight;

            if (var_A != null)
                EditorGUI.PropertyField(position, var_A);

            SerializedProperty comparisonType = property.FindPropertyRelative("m_comparison");
            
            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, comparisonType);

            position.y += EditorGUIUtility.singleLineHeight;
            if (var_B != null)
                EditorGUI.PropertyField(position, var_B);



            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * 5;
        }
    }
}
#endif