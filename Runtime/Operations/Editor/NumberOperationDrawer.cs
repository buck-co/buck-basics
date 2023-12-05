#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    
    [CustomPropertyDrawer(typeof(NumberOperation))]
    public class NumberOperationDrawer : BaseOperationDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            GUIStyle style = base.CenteredLightLabel;

            position.height = EditorGUIUtility.singleLineHeight;
            label = EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            EditorGUI.PrefixLabel(position, label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 2;

            SerializedProperty operation = property.FindPropertyRelative("m_operation");

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, operation);

            
            SerializedProperty rightHandArithmetic = property.FindPropertyRelative("m_rightHandArithmetic");

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, rightHandArithmetic);



            SerializedProperty numberA = property.FindPropertyRelative("m_numberA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (numberA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, numberA);
            
            GUI.backgroundColor = Color.white;//Clear tint if it got set

            
            
            NumberOperation.Operations numOperation = (NumberOperation.Operations)(operation.enumValueIndex);

            position.y += EditorGUIUtility.singleLineHeight;

            switch (numOperation)
            {
                case NumberOperation.Operations.SetTo:
                    base.DrawTextFieldGUI(position, "=", style);
                    break;

                case NumberOperation.Operations.AdditionAssignment:
                    base.DrawTextFieldGUI(position, "+=", style);
                    break;

                case NumberOperation.Operations.SubtractionAssignment:
                    base.DrawTextFieldGUI(position, "-=", style);
                    break;

                case NumberOperation.Operations.MultiplicationAssignment:
                    base.DrawTextFieldGUI(position, "*=", style);
                    break;

                case NumberOperation.Operations.DivisionAssignment:
                    base.DrawTextFieldGUI(position, "/=", style);
                    break;

                case NumberOperation.Operations.PowAssignment:
                    base.DrawTextFieldGUI(position, "^=", style);
                    break;

            }


            
            SerializedProperty numberB = property.FindPropertyRelative("m_numberB");

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, numberB);

            

            NumberOperation.RightHandArithmetic rHandArithmetic = (NumberOperation.RightHandArithmetic)(rightHandArithmetic.enumValueIndex);

            if (rHandArithmetic != NumberOperation.RightHandArithmetic.None)
            {
                position.y += EditorGUIUtility.singleLineHeight;

                switch (rHandArithmetic)
                {
                    case NumberOperation.RightHandArithmetic.Addition:
                        base.DrawTextFieldGUI(position, "+", style);
                        break;

                    case NumberOperation.RightHandArithmetic.Subtraction:
                        base.DrawTextFieldGUI(position, "-", style);
                        break;
                        
                    case NumberOperation.RightHandArithmetic.Multiplication:
                        base.DrawTextFieldGUI(position, "*", style);
                        break;
                        
                    case NumberOperation.RightHandArithmetic.Division:
                        base.DrawTextFieldGUI(position, "/", style);
                        break;
                        
                    case NumberOperation.RightHandArithmetic.Pow:
                        base.DrawTextFieldGUI(position, "^", style);
                        break;

                }

                SerializedProperty numberC = property.FindPropertyRelative("m_numberC");

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, numberC);
            }
         

            IntVariable intVarA = numberA.objectReferenceValue as IntVariable;

            if (intVarA != null)//This is an int variable
            {
                //Show rounding:
                SerializedProperty rounding = property.FindPropertyRelative("m_rounding");

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, rounding);
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
            SerializedProperty numberA = property.FindPropertyRelative("m_numberA");
            IntVariable intVarA = numberA.objectReferenceValue as IntVariable;

            if (intVarA != null)//This is an int variable
                add+=1;//Add an extra line for RoundToIntField


            
            SerializedProperty rightHandArithmetic = property.FindPropertyRelative("m_rightHandArithmetic");
            NumberOperation.RightHandArithmetic rHandArithmetic = (NumberOperation.RightHandArithmetic)(rightHandArithmetic.enumValueIndex);
            
            if (rHandArithmetic != NumberOperation.RightHandArithmetic.None)
            {
                add+=2;//Add two extra lines for number operations that feature a third field and an additional arithmetic
            }
            


            return EditorGUIUtility.singleLineHeight * (7+add);
        }

    }
    
}
#endif