using UnityEngine;

namespace Buck
{
    // Extension Methods that can be used to create gizmos in the Unity Game View
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Draws a wireframe circle in Unity's Game View when the game is running and the gizmo drawing is enabled.
        /// </summary>
        public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color c, float duration = 0f)
        {   
            Vector3 up = Vector3.up;
            
            if (normal == up)
                up = Vector3.right;
            
            int segments = 20;
            Vector3 p1 = Vector3.Cross(up, normal).normalized * radius;

            for(int i = 0; i<segments; i++)
            {
                Vector3 p2 = Quaternion.AngleAxis(360f / (float)segments, normal) * p1;
                Debug.DrawLine(center + p1, center + p2, c, duration);
                p1 = p2;
            }
        }

        /// <summary>
        /// Draws a wireframe sphere (composed of 3 cirles) in Unity's Game View when the game is running and the gizmo drawing is enabled.
        /// </summary>
        public static void DrawSphere(Vector3 center, float radius, Color c, float duration = 0f)
        {   
            DrawCircle(center, Vector3.forward, radius, c, duration);
            DrawCircle(center, Vector3.right, radius, c, duration);
            DrawCircle(center, Vector3.up, radius, c, duration);
        }

        /// <summary>
        /// Draws a wireframe pin (composed of 3 cirles) in Unity's Game View when the game is running and the gizmo drawing is enabled.
        /// </summary>
        public static void DrawPin(Vector3 start, Vector3 end, float radius, Color c, float duration = 0f)
        {      
            Debug.DrawLine(start, end, c, duration);
            DrawCircle(end, Vector3.forward, radius, c, duration);
            DrawCircle(end, Vector3.right, radius, c, duration);
            DrawCircle(end, Vector3.up, radius, c, duration);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Draws a text string in as a gizmo in world space. This only works in the Unity Editor.
        /// </summary>
        public static void DrawGizmoString(this Component component, string text, Vector3 worldPos, Color? color = null)
        {
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            if (view == null) return;

            UnityEditor.Handles.BeginGUI();

            var previousColor = GUI.color;

            if (color.HasValue)
                GUI.color = color.Value;

            Vector3 screenPos = view.camera.WorldToScreenPoint(worldPos);

            if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
            {
                GUI.color = previousColor;
                UnityEditor.Handles.EndGUI();
                return;
            }

            Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
            GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);
            GUI.color = previousColor;
            UnityEditor.Handles.EndGUI();
        }
#endif

    }
}
