#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Buck
{
    public class BaseOperationDrawer : PropertyDrawer
    {
        protected void DrawTextFieldGUI(Rect position, string s, GUIStyle style)
        {
            EditorGUI.LabelField(position, s, style);
        }

        protected GUIStyle CenteredLightLabel
        {
            get
            {
                var style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
                style.normal.textColor = Color.blue; // new Color(.3f, .3f, .3f, 1f);
                return style;
            }
        }
    }
}
#endif