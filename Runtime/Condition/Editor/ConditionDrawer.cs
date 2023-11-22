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


            Condition.VariableType prevVariableType = (Condition.VariableType)(variableType.enumValueIndex);
            
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, variableType);

            Condition.VariableType afterVariableType = (Condition.VariableType)(variableType.enumValueIndex);

            if (prevVariableType != afterVariableType)//The user just changed the Condition variable type
            {
                //Find variables of the type the user just changed from and clear them to null:
                switch (prevVariableType)
                {
                    case Condition.VariableType.Bool:
                        property.FindPropertyRelative("m_boolA").FindPropertyRelative("ConstantValue").boolValue = false;
                        ClearReference(property, "m_boolA");
                        property.FindPropertyRelative("m_boolB").FindPropertyRelative("ConstantValue").boolValue = false;
                        ClearReference(property, "m_boolB");
                    break;

                    case Condition.VariableType.Number:
                        property.FindPropertyRelative("m_numberA").FindPropertyRelative("ConstantValue").floatValue = 0f;
                        ClearReference(property, "m_numberA");
                        property.FindPropertyRelative("m_numberB").FindPropertyRelative("ConstantValue").floatValue = 0f;
                        ClearReference(property, "m_numberB");
                    break;

                    case Condition.VariableType.Vector:
                        property.FindPropertyRelative("m_vectorA").FindPropertyRelative("ConstantValue").vector4Value = Vector4.zero;
                        ClearReference(property, "m_vectorA");
                        property.FindPropertyRelative("m_vectorB").FindPropertyRelative("ConstantValue").vector4Value = Vector4.zero;
                        ClearReference(property, "m_vectorB");
                    break;

                    default:
                    break;
                }
            }
            

            SerializedProperty var_A = null;
            SerializedProperty var_B = null;

            switch (afterVariableType)
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

        /// <summary>
        /// Resets a generic reference property to UseVariable = false and clears the variable to null
        /// </summary>
        /// <param name="rootProperty"></param>
        /// <param name="variableReferenceName"></param>
        void ClearReference(SerializedProperty rootProperty, string variableReferenceName)
        {
            rootProperty.FindPropertyRelative(variableReferenceName).FindPropertyRelative("UseVariable").boolValue = false;
            rootProperty.FindPropertyRelative(variableReferenceName).FindPropertyRelative("Variable").objectReferenceValue = null;
        }

        public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight * 5;
        }
    }
}
#endif