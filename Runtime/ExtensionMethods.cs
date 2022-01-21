using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Shuffle() will effectively randomize the order of elements in a List using the Fisher–Yates shuffle algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, int seed = 0)
        {
            System.Random rng;
            if (seed != 0)
                rng = new System.Random(seed);
            else
                rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Swap() takes two List indices and exchanges their places.
        /// </summary>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        /// <summary>
        /// Returns true if a value is between a minimum (inclusive) and maximum (inclusive).
        /// </summary>
        public static bool IsBetween(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Random() will return a random element from a List.
        /// </summary>
        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Remaps a value from a minimum and maximum range to another minimum and maximum range.
        /// </summary>
        public static float Remap (this float value, float fromMin, float fromMax, float toMin, float toMax) {
            return Mathf.Clamp((value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin, toMin, toMax);
        }

        /// <summary>
        /// Remaps a value from a minimum and maximum range to a zero to one range.
        /// </summary>
        public static float Remap01 (this float value, float min, float max) {
            return Remap(value, min, max, 0, 1);
        }

        /// <summary>
        /// Rounds a float to a specified number of digits after the decimal point.
        /// </summary>
        public static float Round(this float value, int digits)
        {
            return (float)System.Math.Round(value, digits);
            /*float mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round(value * mult) / mult;*/
        }

        /// <summary>
        /// Rounds a double to a specified number of digits after the decimal point.
        /// </summary>
        public static double Round(this double value, int digits)
        {
            return System.Math.Round(value, digits);
            /*float mult = Mathf.Pow(10.0f, digits);
            return Mathf.Round((float)value * mult) / mult;*/
        }

        /// <summary>
        /// Performs Pow while keeping Sign
        /// </summary>
        public static float SignedPow( float value, float pow )
        {
            return Mathf.Pow( Mathf.Abs( value ), pow ) * Mathf.Sign( value );
        }

        /// <summary>
        /// Returns true if Bounds are visible from the provided camera.
        /// </summary>
        public static bool IsVisibleFrom(this Bounds bounds, Camera camera)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            return GeometryUtility.TestPlanesAABB(planes, bounds);
        }

        /// <summary>
        /// Returns true if a Renderer is visible from the provided camera.
        /// </summary>
        public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
        {
            return IsVisibleFrom(renderer.bounds, camera);
        }

        /// <summary>
        /// Returns a transposed 2D array (swaps rows and columns)
        /// </summary>
        public static T[,] Transpose<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] transposed = new T[columnCount, rowCount];   
        
            for (int column = 0; column < columnCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    transposed[column, row] = arr[row, column];
                }
            }
            return transposed;
        }
        /// <summary>
        /// Returns a 2D array with its column values reversed
        /// </summary>
        public static T[,] ReverseColumns<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    reversed[row, columnCount - 1 - column ] = arr[row, column];
                }
            }
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array with its row values reversed
        /// </summary>
        public static T[,] ReverseRows<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
            {
                for (int row = 0; row < rowCount; row++)
                {
                    reversed[ rowCount - 1 - row, column] = arr[row, column];
                }
            }
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 90 degrees CW
        /// </summary>
        public static T[,] Rotate90<T>(this T[,] arr )
        {
            return arr.Transpose().ReverseRows();
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 270 degrees CW (or 90 degrees CCW)
        /// </summary>
        public static T[,] Rotate270<T>(this T[,] arr )
        {
            return arr.Transpose().ReverseColumns();
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 180 degrees
        /// </summary>
        public static T[,] Rotate180<T>(this T[,] arr )
        {
            return arr.ReverseColumns().ReverseRows();
        }

        /// <summary>
        /// Returns a BoundsInt with Y set to zero.
        /// </summary>
        public static BoundsInt BoundsIntFromXZ(int X, int Z, int Width, int Depth)
        {
            return new BoundsInt(new Vector3Int(X, 0, Z), new Vector3Int(Width, 0, Depth));
        }

        /// <summary>
        /// Returns a BoundsInt with Y set to zero.
        /// </summary>
        public static BoundsInt BoundsIntFromXZ(Vector2Int position, Vector2Int size)
        {
            return BoundsIntFromXZ(position.x, position.y, size.x, size.y);
        }

        /// <summary>
        /// Returns true if the provided BoundsInt overlaps in the X and Z space.
        /// </summary>
        public static bool IntersectsXZ(this BoundsInt A, BoundsInt B)
        {
            return (A.min.x <= B.max.x) && (A.max.x >= B.min.x) &&
                   (A.min.z <= B.max.z) && (A.max.z >= B.min.z);
        }

        /// <summary>
        /// Returns true if the provided BoundsInt overlaps in the X and Z space.
        /// </summary>
        public static bool IntersectsXZ(this BoundsInt bounds, int bX, int bZ, int bWidth, int bDepth)
        {
            return bounds.IntersectsXZ(BoundsIntFromXZ(bX, bZ, bWidth, bDepth));
        }

        /// <summary>
        /// Returns a new BoundsInt of the X and Z intersection between two BoundsInts.
        /// </summary>
        public static BoundsInt GetIntersectionXZ(this BoundsInt A, BoundsInt B)
        {
            int X = Mathf.Max(A.x, B.x);
            int Z = Mathf.Max(A.z, B.z);
            int ExtentX = Mathf.Min(A.x + A.size.x, B.x + B.size.x);
            int ExtentZ = Mathf.Min(A.z + A.size.z, B.z + B.size.z);
            return BoundsIntFromXZ(X, Z, ExtentX - X, ExtentZ - Z);
        }

        /// <summary>
        /// Returns a new BoundsInt of the X and Z intersection between two BoundsInts.
        /// </summary>
        public static BoundsInt GetIntersectionXZ(this BoundsInt A, int bX, int bZ, int bWidth, int bDepth)
        {
            return A.GetIntersectionXZ(BoundsIntFromXZ(bX, bZ, bWidth, bDepth));
        }

        /// <summary>
        /// Given two AABBs (center pos, half widths) returns true if shapes overlap.
        /// </summary>
        public static bool AABBvsAABB(Vector2 aPos, Vector2 aHalfWidths, Vector2 bPos, Vector2 bHalfWidths)
        {
            return aPos.x - aHalfWidths.x <= bPos.x + bHalfWidths.x && 
                   aPos.x + aHalfWidths.x >= bPos.x - bHalfWidths.x && 
                   aPos.y - aHalfWidths.y <= bPos.y + bHalfWidths.y && 
                   aPos.y + aHalfWidths.y >= bPos.y - bHalfWidths.y;
        } 

        /// <summary>
        /// Given an AABB (center pos, half widths) and a circle (center pos, radius), returns true if shapes overlap.
        /// </summary>
        public static bool AABBvsCircle(Vector2 aPos, Vector2 aHalfWidths, Vector2 bPos, float bRadius)
        {
            Vector2 difference = bPos - aPos;
            Vector2 clamped = new Vector2(Mathf.Clamp(difference.x, -aHalfWidths.x, aHalfWidths.x), Mathf.Clamp(difference.y, -aHalfWidths.y, aHalfWidths.y));
            Vector2 closest = aPos + clamped;
            difference = closest - bPos;
            return difference.sqrMagnitude <= (bRadius * bRadius);
        }

        /// <summary>
        /// Draws a debug circle.
        /// </summary>
        public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color c, float duration = 0f )
        {   
            Vector3 up = Vector3.up;
            if(normal == up) up = Vector3.right;
            int segments = 20;
            Vector3 p1 = Vector3.Cross(up, normal).normalized * radius;

            for(int i = 0; i<segments; i++)
            {
                Vector3 p2 = Quaternion.AngleAxis(360f / (float)segments, normal) * p1;
                Debug.DrawLine(center + p1, center + p2, c, duration );
                p1 = p2;
            }
        }

        public static void DrawSphere(Vector3 center, float radius, Color c, float duration = 0f)
        {   
            DrawCircle(center, Vector3.forward, radius, c, duration);
            DrawCircle(center, Vector3.right, radius, c, duration);
            DrawCircle(center, Vector3.up, radius, c, duration);
        }

        
        public static void DrawPin( Vector3 start, Vector3 end, float radius, Color c, float duration = 0f)
        {      
            Debug.DrawLine( start, end, c, duration );
            DrawCircle(end, Vector3.forward, radius, c, duration);
            DrawCircle(end, Vector3.right, radius, c, duration);
            DrawCircle(end, Vector3.up, radius, c, duration);
        }

        /// <summary>
        /// Adds a tint to a Color.
        /// </summary>
        public static Color Tint(this Color value, float tint)
        {
            return new Color(Mathf.Clamp01(value.r + (tint * (1 - value.r))),
                             Mathf.Clamp01(value.g + (tint * (1 - value.g))),
                             Mathf.Clamp01(value.b + (tint * (1 - value.b))),
                             value.a);
        }

        /// <summary>
        /// Sets the alpha of a Color.
        /// </summary>
        public static Color SetAlpha(this Color value, float alpha)
        {
            return new Color(value.r,
                             value.g,
                             value.b,
                             alpha);
        }

        /// <summary>
        /// Returns a random point on a unit circle.
        /// </summary>
        public static Vector2 RandomPointOnUnitCircle()
        {
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2f);
            return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        /// <summary>
        /// Returns a random point on a 2D ring.
        /// </summary>
        public static Vector2 RandomPointOnRing(float innerRadius, float outerRadius)
        {
            return RandomPointOnUnitCircle() * UnityEngine.Random.Range(innerRadius, outerRadius);
        }

        /// <summary>
        /// Given a list of Transforms, return the one that is nearest to an origin Transform.
        /// If there are no positions, returns null. If there's 1 position, returns it.
        /// </summary>
        public static Transform NearestTransform(this Transform origin, List<Transform> positions)
        {
            if (positions.Count == 0)
                return null;

            if (positions.Count == 1)
                return positions[0];

            Transform nearestTransform = null;
            float nearestSqDistance = Mathf.Infinity;
            foreach (Transform t in positions)
            {
                float sqDistanceToPosition = (t.position - origin.position).sqrMagnitude;
                if (sqDistanceToPosition < nearestSqDistance)
                {
                    nearestSqDistance = sqDistanceToPosition;
                    nearestTransform = t;
                }
            }

            return nearestTransform;
        }

        /// <summary>
        /// Given a list of MonoBehaviours, return the one with the transform that is nearest to an origin Transform.
        /// If there are no MonoBehaviours, returns null. If there's 1 MonoBehaviour, returns its transform.
        /// </summary>
        public static Transform NearestTransform(this Transform origin, List<MonoBehaviour> behaviours)
        {
            List<Transform> transforms = new List<Transform>();
            foreach (MonoBehaviour mb in behaviours)
                transforms.Add(mb.transform);
            return NearestTransform(origin, transforms);
        }

        /// <summary>
        /// Returns a Gradient that goes from white to black linearly.
        /// </summary>
        public static Gradient WhiteToBlackGradient
        {
            get
            {
                Gradient g = new Gradient();
                g.colorKeys = new GradientColorKey[2] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.black, 1) };
                return g;
            }
        }

        /// <summary>
        /// Ken Perlin's better smooth step with 1st and 2nd order derivatives at x = 0 and 1
        /// </summary>
        public static float Smootherstep(float from, float to, float x) 
        {
            x = Mathf.Clamp01( x );
            //6x^5 - 15x^2 + 10x^3
            return Mathf.Lerp( from, to, x * x * x * (x * (x * 6 - 15) + 10) );   
        }

        /// <summary>
        /// Get the volume of a sphere from its radius.
        /// </summary>
        public static float SphereVolume(float r)
        {
            return (4f/3f)*Mathf.PI*r*r*r;
        }

        /// <summary>
        /// Get the surface area of a sphere from its radius.
        /// </summary>
        public static float SphereSurfaceArea(float r)
        {
            return 4f * Mathf.PI * r * r;
        }

        /// <summary>
        /// Get the volume of a box from its size.
        /// </summary>
        public static float BoxVolume(Vector3 size)
        {
            return size.x*size.y*size.z;
        }

        /// <summary>
        /// Get the surface area of a box from its size.
        /// </summary>
        public static float BoxSurfaceArea(Vector3 size)
        {
            return 2f * ((size.x * size.z) + (size.x * size.y) + (size.y * size.z));
        }

        /// <summary>
        /// Returns true if the signs of two floats are the same.
        /// </summary>
        public static bool SameSign(float a, float b)
        {
            return a * b >= 0.0f;
        }

        /// <summary>
        /// Returns a Vector3 position along a parabola as a function of time.
        /// </summary>
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            float arc = -4 * height * t * t + 4 * height * t;
            Vector3 mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, arc + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        /// <summary>
        /// Adds an explosive force to all Rigidbody components in a specified radius.
        /// </summary>
        public static void ExplosiveForce(Vector3 origin, float radius, float force, float upwardsModifier = 3f)
        {
            Collider[] colliders = Physics.OverlapSphere(origin, radius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(force, origin, radius, upwardsModifier);
            }
        }

        #if UNITY_EDITOR
        static public void DrawGizmoString(this Component component, string text, Vector3 worldPos, Color? color = null)
        {
            var view = UnityEditor.SceneView.currentDrawingSceneView;
            if (view == null) return;

            UnityEditor.Handles.BeginGUI();

            var previousColor = GUI.color;

            if (color.HasValue) GUI.color = color.Value;

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