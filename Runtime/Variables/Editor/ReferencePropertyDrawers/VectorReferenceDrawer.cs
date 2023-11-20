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
        protected override void CreateValueGUI(Rect position, SerializedProperty useConstant, SerializedProperty constantValue, SerializedProperty variable)
        {
            //Vector4 needs to overwrite how this created with GUI since by default unity will create Vector4 as a field where x,y,z,w are all on different lines.
            //This puts them in a single line like how Vector3 acts.
            if (useConstant.boolValue)
            {
                constantValue.vector4Value = EditorGUI.Vector4Field(position, "", constantValue.vector4Value); 
            }
            else
            {
                EditorGUI.PropertyField(position, variable, GUIContent.none);
            }
            
        }
    }
}
#endif