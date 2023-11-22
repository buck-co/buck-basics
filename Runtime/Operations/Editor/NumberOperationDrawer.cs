//#if UNITY_EDITOR
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

            SerializedProperty useMultiplier = property.FindPropertyRelative("m_useMultiplier");
            bool usingMultiplier = BoolReferencePropertyIsTrue(useMultiplier);

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, useMultiplier);


            SerializedProperty numberA = property.FindPropertyRelative("m_numberA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (numberA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, numberA);
            
            GUI.backgroundColor = Color.white;//Clear tint if it got set

            
            
            NumberOperation.Operations numOperation = (NumberOperation.Operations)(operation.enumValueIndex);

            bool drawNumC = false;
            string bToCOperation ="";//Only used if drawing num c

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

                case NumberOperation.Operations.Addition:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumC = true;
                    bToCOperation = "+";
                    break;

                case NumberOperation.Operations.Subtraction:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumC = true;
                    bToCOperation = "-"; 
                    break;
                    
                case NumberOperation.Operations.Multiplication:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumC = true;
                    bToCOperation = "*";
                    break;

                case NumberOperation.Operations.Division:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumC = true;
                    bToCOperation = "/";
                    break;
                    
                case NumberOperation.Operations.Pow:
                    base.DrawTextFieldGUI(position, "=", style);
                    drawNumC = true;
                    bToCOperation = "^"; 
                    break;


            }

            
            if (usingMultiplier)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                base.DrawTextFieldGUI(position, "(", style);
            }

            
            SerializedProperty numberB = property.FindPropertyRelative("m_numberB");

            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, numberB);
         
            if (drawNumC)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                base.DrawTextFieldGUI(position, bToCOperation, style);
                
                SerializedProperty numberC = property.FindPropertyRelative("m_numberC");

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, numberC);
            }

            if (usingMultiplier)
            {
                position.y += EditorGUIUtility.singleLineHeight;
                base.DrawTextFieldGUI(position, ")", style);
                position.y += EditorGUIUtility.singleLineHeight;
                base.DrawTextFieldGUI(position, "*", style);


                
                SerializedProperty numberMultiplier = property.FindPropertyRelative("m_numberMultiplier");

                position.y += EditorGUIUtility.singleLineHeight;
                EditorGUI.PropertyField(position, numberMultiplier);
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


            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            NumberOperation.Operations numOperation = (NumberOperation.Operations)(operation.enumValueIndex);

            if (numOperation == NumberOperation.Operations.Addition ||
                numOperation == NumberOperation.Operations.Subtraction ||
                numOperation == NumberOperation.Operations.Multiplication ||
                numOperation == NumberOperation.Operations.Division ||
                numOperation == NumberOperation.Operations.Pow
            )
                add+=2;//Add two extra lines for number operations that feature to more fields of lines (numvarC and bToCOperation)

            

            bool useMultiply = BoolReferencePropertyIsTrue(property.FindPropertyRelative("m_useMultiplier"));

            if (useMultiply)
            {
                add+=4;//Add 3 extra lines if we are using multiplier
            }


            return EditorGUIUtility.singleLineHeight * (7+add);
        }

        public bool BoolReferencePropertyIsTrue(SerializedProperty boolReferenceSerializedProperty)
        {
            
            if (!boolReferenceSerializedProperty.FindPropertyRelative("UseVariable").boolValue)
            {
                if (boolReferenceSerializedProperty.FindPropertyRelative("ConstantValue").boolValue)
                    return true;
            }
            else
            {
                return true;//If using a variable we want to display the multiplier even if the current state is false (could become true at runtime)
            }

            return false;
        }

    }
}
//#endif