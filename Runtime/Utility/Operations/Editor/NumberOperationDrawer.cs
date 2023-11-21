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
            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            EditorGUILayout.PropertyField(operation);

            SerializedProperty numberA = property.FindPropertyRelative("m_numberA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (numberA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

            EditorGUILayout.PropertyField(numberA);

            GUI.backgroundColor = Color.white;//Clear tint if it got set
            
            GUIStyle style = base.CenteredLightLabel;

            NumberOperation.Operations numOperation = (NumberOperation.Operations)(operation.enumValueIndex);

            bool drawNumC = false;
            string bToCOperation ="";//Only used if drawing num c



            switch (numOperation)
            {
                case NumberOperation.Operations.SetTo:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    break;

                case NumberOperation.Operations.AdditionAssignment:
                    EditorGUILayout.LabelField("+=", style, GUILayout.ExpandWidth(true));
                    break;

                case NumberOperation.Operations.SubtractionAssignment:
                    EditorGUILayout.LabelField("-=", style, GUILayout.ExpandWidth(true));
                    break;

                case NumberOperation.Operations.MultiplicationAssignment:
                    EditorGUILayout.LabelField("*=", style, GUILayout.ExpandWidth(true)); 
                    break;

                case NumberOperation.Operations.DivisionAssignment:
                    EditorGUILayout.LabelField("/=", style, GUILayout.ExpandWidth(true)); 
                    break;

                case NumberOperation.Operations.PowAssignment:
                    EditorGUILayout.LabelField("^=", style, GUILayout.ExpandWidth(true)); 
                    break;

                case NumberOperation.Operations.Addition:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    drawNumC = true;
                    bToCOperation = "+";
                    break;

                case NumberOperation.Operations.Subtraction:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    drawNumC = true;
                    bToCOperation = "-"; 
                    break;
                    
                case NumberOperation.Operations.Multiplication:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true)); 
                    drawNumC = true;
                    bToCOperation = "*";
                    break;

                case NumberOperation.Operations.Division:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true)); 
                    drawNumC = true;
                    bToCOperation = "/";
                    break;
                    
                case NumberOperation.Operations.Pow:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    drawNumC = true;
                    bToCOperation = "^"; 
                    break;


            }

            
            SerializedProperty numberB = property.FindPropertyRelative("m_numberB");
            EditorGUILayout.PropertyField(numberB);
         
            if (drawNumC)
            {
                EditorGUILayout.LabelField(bToCOperation, style, GUILayout.ExpandWidth(true));
                
                SerializedProperty numberC = property.FindPropertyRelative("m_numberC");
                EditorGUILayout.PropertyField(numberC);
            }

            IntVariable intVarA = numberA.objectReferenceValue as IntVariable;

            if (intVarA != null)//This is an int variable
            {
                //Show rounding:
                SerializedProperty rounding = property.FindPropertyRelative("m_rounding");
                EditorGUILayout.PropertyField(rounding);
            }

            SerializedProperty raiseEvent = property.FindPropertyRelative("m_raiseEvent");
            EditorGUILayout.PropertyField(raiseEvent);
        }

    }
}
#endif