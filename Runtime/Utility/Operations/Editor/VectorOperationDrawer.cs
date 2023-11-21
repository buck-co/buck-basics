#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(VectorOperation))]
    public class VectorOperationDrawer : BaseOperationDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            SerializedProperty operation = property.FindPropertyRelative("m_operation");
            EditorGUILayout.PropertyField(operation);

            SerializedProperty vectorA = property.FindPropertyRelative("m_vectorA");

            //Tint the A variable's background color light red if the variable is left null, to help clue the user into errors
            if (vectorA.objectReferenceValue == null)
                GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);
                
            EditorGUILayout.PropertyField(vectorA);

            GUI.backgroundColor = Color.white;//Clear tint if it got set
            
            GUIStyle style = base.CenteredLightLabel;

            VectorOperation.Operations vectorOperation = (VectorOperation.Operations)(operation.enumValueIndex);

            bool drawVecC = false;
            bool drawNumScalar = false;
            string bToCOperation ="";//Only used if drawing num c



            switch (vectorOperation)
            {
                case VectorOperation.Operations.SetTo:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    break;

                case VectorOperation.Operations.AdditionAssignment:
                    EditorGUILayout.LabelField("+=", style, GUILayout.ExpandWidth(true));
                    break;

                case VectorOperation.Operations.SubtractionAssignment:
                    EditorGUILayout.LabelField("-=", style, GUILayout.ExpandWidth(true));
                    break;
                    
                case VectorOperation.Operations.Addition:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    drawVecC = true;
                    bToCOperation = "+";
                    break;

                case VectorOperation.Operations.Subtraction:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true));
                    drawVecC = true;
                    bToCOperation = "-"; 
                    break;
                    
                case VectorOperation.Operations.MultiplyByScalar:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true)); 
                    drawNumScalar = true;
                    bToCOperation = "*";
                    break;

                case VectorOperation.Operations.DivideByScalar:
                    EditorGUILayout.LabelField("=", style, GUILayout.ExpandWidth(true)); 
                    drawNumScalar = true;
                    bToCOperation = "/";
                    break;

            }
            
            SerializedProperty vectorB = property.FindPropertyRelative("m_vectorB");
            EditorGUILayout.PropertyField(vectorB);
         

            if (drawVecC || drawNumScalar)
            {
                EditorGUILayout.LabelField(bToCOperation, style, GUILayout.ExpandWidth(true));
            }

            if (drawVecC)
            {
                SerializedProperty vectorC = property.FindPropertyRelative("m_vectorC");
                EditorGUILayout.PropertyField(vectorC);
            }

            if (drawNumScalar)
            {
                SerializedProperty numberScalar = property.FindPropertyRelative("m_numberScalar");
                EditorGUILayout.PropertyField(numberScalar);
            }
            
            SerializedProperty raiseEvent = property.FindPropertyRelative("m_raiseEvent");
            EditorGUILayout.PropertyField(raiseEvent);

        }

    }
}
#endif