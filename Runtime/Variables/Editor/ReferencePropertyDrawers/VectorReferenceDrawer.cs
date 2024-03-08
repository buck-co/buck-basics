#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    [CustomPropertyDrawer(typeof(Vector4Reference))]
    public class Vector4ReferenceDrawer:VectorReferenceDrawer{}


    [CustomPropertyDrawer(typeof(VectorReference))]
    public class VectorReferenceDrawer : BaseReferenceDrawer
    {
        protected override void CreateValueGUI(Rect position, SerializedProperty useVariable, SerializedProperty constantValue, SerializedProperty variable)
        {
            // Vector4 needs to overwrite how this created with GUI since by default unity will create Vector4 as a field where x,y,z,w are all on different lines.
            // This puts them in a single line like how Vector3 acts.
            if (useVariable.boolValue)
            {
                // Tint field light red if this Reference is using the variable but the variable is null (likely to create errors)
                if (variable.objectReferenceValue == null)
                    GUI.backgroundColor = new Color(1f, .75f, .75f, 1f);

                EditorGUI.PropertyField(position, variable, GUIContent.none);
            }
            else
                constantValue.vector4Value = EditorGUI.Vector4Field(position, "", constantValue.vector4Value); 
        }
    }
}
#endif
