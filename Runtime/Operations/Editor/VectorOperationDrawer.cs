//#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(VectorOperation))]
    public class VectorOperationDrawer : BaseOperationDrawer
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

            SerializedProperty vectorA = property.FindPropertyRelative("m_vectorA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (vectorA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);
            
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, vectorA);

            GUI.backgroundColor = Color.white;//Clear tint if it got set
            
            GUIStyle style = base.CenteredLightLabel;

            VectorOperation.Operations vectorOperation = (VectorOperation.Operations)(operation.enumValueIndex);

            bool drawVecC = false;
            bool drawNumScalar = false;
            string bToCOperation ="";//Only used if drawing num c


            position.y += EditorGUIUtility.singleLineHeight;

            switch (vectorOperation)
            {
                case VectorOperation.Operations.SetTo:
                    base.DrawTextFieldGUI(position, "=", style);
                    break;

                case VectorOperation.Operations.AdditionAssignment:
                    base.DrawTextFieldGUI(position, "+=", style);
                    break;

                case VectorOperation.Operations.SubtractionAssignment:
                    base.DrawTextFieldGUI(position, "-=", style);
                    break;
                    
                case VectorOperation.Operations.Addition:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawVecC = true;
                    bToCOperation = "+";
                    break;

                case VectorOperation.Operations.Subtraction:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawVecC = true;
                    bToCOperation = "-"; 
                    break;
                    
                case VectorOperation.Operations.MultiplyByScalar:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumScalar = true;
                    bToCOperation = "*";
                    break;

                case VectorOperation.Operations.DivideByScalar:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumScalar = true;
                    bToCOperation = "/";
                    break;

            }
            
            SerializedProperty vectorB = property.FindPropertyRelative("m_vectorB");

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, vectorB);
         

            if (drawVecC || drawNumScalar)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                base.DrawTextFieldGUI(position, bToCOperation, style);
            }

            if (drawVecC)
            {
                SerializedProperty vectorC = property.FindPropertyRelative("m_vectorC");

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, vectorC);
            }

            if (drawNumScalar)
            {
                SerializedProperty numberScalar = property.FindPropertyRelative("m_numberScalar");
                
                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, numberScalar);
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
            

            int add = 0;

            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            VectorOperation.Operations vecOperation = (VectorOperation.Operations)(operation.enumValueIndex);

            if (vecOperation == VectorOperation.Operations.Addition ||
                vecOperation == VectorOperation.Operations.Subtraction ||
                vecOperation == VectorOperation.Operations.MultiplyByScalar ||
                vecOperation == VectorOperation.Operations.DivideByScalar
            )
                add+=2;//Add two extra lines for number operations that feature to more fields of lines (numvarC and bToCOperation)



            return EditorGUIUtility.singleLineHeight * (6+add);
        }

    }
}
//#endif