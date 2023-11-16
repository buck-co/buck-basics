using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Effectively randomizes the order of elements in a List using the Fisher–Yates shuffle algorithm.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, int seed = 0)
        {
            System.Random rng = (seed != 0) ? new System.Random(seed) : new System.Random();

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
        /// Exchange the places of two given List indices.
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
        /// Returns a random element from a List.
        /// </summary>
        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Remaps a value from a minimum and maximum range to another minimum and maximum range.
        /// </summary>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
        {
            return Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));
        }

        /// <summary>
        /// Remaps a value from a minimum and maximum range to a zero to one range.
        /// </summary>
        public static float Remap01(this float value, float min, float max)
        {
            return Remap(value, min, max, 0, 1);
        }

        /// <summary>
        /// Rounds a float to a specified number of digits after the decimal point.
        /// </summary>
        public static float Round(this float value, int digits)
        {
            return (float)System.Math.Round(value, digits);
        }

        /// <summary>
        /// Rounds a double to a specified number of digits after the decimal point.
        /// </summary>
        public static double Round(this double value, int digits)
        {
            return System.Math.Round(value, digits);
        }

        /// <summary>
        /// Raises a float value by a power and keeps its sign (positive or negative).
        /// </summary>
        public static float SignedPow(float value, float pow)
        {
            return Mathf.Pow(Mathf.Abs(value), pow) * Mathf.Sign(value);
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
        /// Returns a transposed 2D array (swaps rows and columns).
        /// </summary>
        public static T[,] Transpose<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] transposed = new T[columnCount, rowCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    transposed[column, row] = arr[row, column];
            
            return transposed;
        }
        /// <summary>
        /// Returns a 2D array with its column values reversed.
        /// </summary>
        public static T[,] ReverseColumns<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    reversed[row, columnCount - 1 - column] = arr[row, column];
            
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array with its row values reversed.
        /// </summary>
        public static T[,] ReverseRows<T>(this T[,] arr)
        {
            int rowCount = arr.GetLength(0);
            int columnCount = arr.GetLength(1);
            T[,] reversed = new T[rowCount, columnCount];   
        
            for (int column = 0; column < columnCount; column++)
                for (int row = 0; row < rowCount; row++)
                    reversed[rowCount - 1 - row, column] = arr[row, column];
            
            return reversed;
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 90 degrees CW.
        /// </summary>
        public static T[,] Rotate90<T>(this T[,] arr)
        {
            return arr.Transpose().ReverseRows();
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 270 degrees CW (or 90 degrees CCW).
        /// </summary>
        public static T[,] Rotate270<T>(this T[,] arr)
        {
            return arr.Transpose().ReverseColumns();
        }
        /// <summary>
        /// Returns a 2D array that has been rotated 180 degrees.
        /// </summary>
        public static T[,] Rotate180<T>(this T[,] arr)
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
        /// Draws a wireframe circle in Unity's Game View when the game is running and the gizmo drawing is enabled.
        /// </summary>
        public static void DrawCircle(Vector3 center, Vector3 normal, float radius, Color c, float duration = 0f)
        {   
            Vector3 up = Vector3.up;
            if(normal == up) up = Vector3.right;
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
        /// Multiples each R, G, B, and A value of one Color by another Color's respective R,G,B, and A values and clamps them 0-1
        /// </summary>
        public static Color Multiply(this Color a, Color b)
        {
            return new Color (Mathf.Clamp01(a.r * b.r),
                             Mathf.Clamp01(a.g * b.g),
                             Mathf.Clamp01(a.b * b.b),
                             Mathf.Clamp01(a.a * b.a)
            );
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



        //*****[Float animation methods]*****
        public static float EaseIn(float t) => 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        public static float EaseOut(float t) => Mathf.Sin(t * Mathf.PI * 0.5f);
        public static float Smoothstep(float t) => t * t * (3f - 2f * t);
        
        /// <summary>
        /// Ken Perlin's better smooth step with 1st and 2nd order derivatives at x = 0 and 1.
        /// </summary>
        public static float Smootherstep(float from, float to, float x) 
        {
            x = Mathf.Clamp01(x);
            return Mathf.Lerp(from, to, x * x * x * (x * (x * 6 - 15) + 10));
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

        
        /// <summary>
        /// Quick method for showing or hiding a CanvasGroup. Effects alpha, blocksraycasts, and interacteable properties. Possible to optionally not effect all three by passing in false for parameters in extended signature.
        /// </summary>
        /// <param name="canvasGroup">The canvas group this call effects.</param>
        /// <param name="on">Whether the effected elements of the canvas group should be on or off.</param>
        /// <param name="effectAlpha">Whether the call will effect alpha. Alpha set to 1f when on is true and 0f when false</param>
        /// <param name="effectInteractable">Whether the call will effect whether the canvas group is interactable</param>
        /// <param name="effectBlocksRaycasts">Whether the call will effect whether the canvas group blocks raycasts</param>
        public static void SetVisible(this CanvasGroup canvasGroup, bool on, bool effectAlpha = true, bool effectInteractable = true, bool effectBlocksRaycasts = true) {
            if (effectAlpha)
                canvasGroup.alpha = (on)? 1f: 0f;
            if (effectInteractable)
                canvasGroup.interactable = on;
            if (effectBlocksRaycasts)
                canvasGroup.blocksRaycasts = on;
        }

        //*****[VECTOR CONVERSION METHODS]*****
        
        //=======[Vector2]========

        /// <summary>
        /// Reconstructs a Vector2 as a Vector2Int rounding each value to the nearest int
        /// </summary>
        public static Vector2Int ToVector2Int(this Vector2 v2) => new Vector2Int(UnityEngine.Mathf.RoundToInt(v2.x), UnityEngine.Mathf.RoundToInt(v2.y));

        /// <summary>
        /// Reconstructs a Vector2Int as a Vector2, casting x and y to floats
        /// </summary>
        public static Vector2 ToVector2(this Vector2Int v2Int) => new Vector2((float)(v2Int.x), (float)(v2Int.y));


        //=======[Vector3]========

        /// <summary>
        /// Reconstructs a Vector3 as a Vector3Int rounding each value to the nearest int
        /// </summary>
        public static Vector3Int ToVector3Int(this Vector3 v3) => new Vector3Int(UnityEngine.Mathf.RoundToInt(v3.x), UnityEngine.Mathf.RoundToInt(v3.y), UnityEngine.Mathf.RoundToInt(v3.z));

        /// <summary>
        /// Reconstructs a Vector3Int as a Vector3, casting x, y, and z to floats
        /// </summary>
        public static Vector3 ToVector3(this Vector3Int v3Int) => new Vector3((float)(v3Int.x), (float)(v3Int.y), (float)(v3Int.z));



        /// <summary>
        /// Finds the Manhattan Distance between a Vector2Int a, and a Vector2Int b. The manhattan distance is the sum of the horizontal and vertical distance betwen two points. The metaphor being that you are travelling along the blocks of the square grid of Manhattan.
        /// </summary>
        public static int ManhattanDistance(Vector2Int a, Vector2Int b) => ManhattanDistance(new Vector3Int(a.x, a.y, 0), new Vector3Int(b.x, b.y, 0));
        
        /// <summary>
        /// Finds the Manhattan Distance between a Vector3Int a, and a Vector3Int b. The manhattan distance is the sum of the horizontal and vertical distance betwen two points. The metaphor being that you are travelling along the blocks of the square grid of Manhattan.
        /// </summary>
        public static int ManhattanDistance(Vector3Int a, Vector3Int b)
        {
            int distance = Mathf.Abs(a.x - b.x);
            distance += Mathf.Abs(a.y - b.y);
            distance += Mathf.Abs(a.z - b.z);
            return distance;
        }

        

        //*****[Rects, Bounds, RectTransforms, and Screen Space]*****
        /// <summary>
        /// Converts a RectTransform into a Bounds
        /// </summary>
        public static Bounds GetRectTransformBounds(RectTransform rectTransform)
        {
            Vector3 min = Vector3.positiveInfinity;
            Vector3 max = Vector3.negativeInfinity;

            // Get the 4 corners in world coordinates
            Vector3[] rectTransformWorldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(rectTransformWorldCorners);

            // Find the min and max corners
            foreach (Vector3 v in rectTransformWorldCorners)
            {
                min = Vector3.Min(min, v);
                max = Vector3.Max(max, v);
            }

            // Set the bounds using the min and max
            Bounds bounds = new Bounds();
            bounds.SetMinMax(min, max);
            
            return bounds;
        }

        /// <summary>
        /// Creates a Rect that uses the min and max of the provided Bounds to define its area.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static Rect GetBoundsRectangle(Bounds bounds)
            => Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);



        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(Renderer renderer, Camera camera = null)
            => GetScreenRectangle(renderer.bounds, camera);
        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(RectTransform rectTransform, Camera camera = null)
            => GetScreenRectangle(GetRectTransformBounds(rectTransform), camera);

        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(Bounds bounds, Camera camera = null)
        {
            if (camera == null)
                return Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);
            
            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;
            Vector3[] screenSpaceCorners = new Vector3[8];

            // For each of the 8 corners of our renderer's world space bounding box,
            // convert those corners into screen space.
            screenSpaceCorners[0] = camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y + e.y, c.z + e.z));
            screenSpaceCorners[1] = camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y + e.y, c.z - e.z));
            screenSpaceCorners[2] = camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z + e.z));
            screenSpaceCorners[3] = camera.WorldToScreenPoint(new Vector3(c.x + e.x, c.y - e.y, c.z - e.z));
            screenSpaceCorners[4] = camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z + e.z));
            screenSpaceCorners[5] = camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y + e.y, c.z - e.z));
            screenSpaceCorners[6] = camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y - e.y, c.z + e.z));
            screenSpaceCorners[7] = camera.WorldToScreenPoint(new Vector3(c.x - e.x, c.y - e.y, c.z - e.z));

            // Now find the min/max X & Y of these screen space corners.
            
            // Start by assuming the min and max is the first corner.
            float minX = screenSpaceCorners[0].x;
            float minY = screenSpaceCorners[0].y;
            float maxX = screenSpaceCorners[0].x;
            float maxY = screenSpaceCorners[0].y;

            // Then, continue looping starting at 1 because the
            // corner at index 0 is already accounted for.
            for (int i = 1; i < 8; i++)
            {
                if(screenSpaceCorners[i].x < minX)
                    minX = screenSpaceCorners[i].x;
                
                if(screenSpaceCorners[i].y < minY)
                    minY = screenSpaceCorners[i].y;
                
                if(screenSpaceCorners[i].x > maxX)
                    maxX = screenSpaceCorners[i].x;
                
                if(screenSpaceCorners[i].y > maxY)
                    maxY = screenSpaceCorners[i].y;
            }
            
            // Then return the Rect using these corners.
            return Rect.MinMaxRect(minX, minY, maxX, maxY);
        }
        //*****[String methods]*****

        /// <summary>
        /// Returns the string shortened to the max length. Optionally, you can add an elipsis ("...") to the end. If the string is shorter than the max length it will return itself and not add the elipsis.
        /// </summary>
        /// <param name="value">The original string to truncate.</param>
        /// <param name="maxLength">How many characters the string should be shortened to.</param>
        /// <param name="addEllipsis">If true and the string is shortened, will add "..." to the end.</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength, bool addEllipsis = false)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + (addEllipsis ? "..." : "");
        }


        //*****[GUID methods]*****

        /// <summary>
        /// Returns a new Guid byte array if the given Guid is empty,
        /// otherwise returns the given Guid.
        /// </summary>
        public static byte[] GetSerializableGuid(System.Guid guid)
            => guid != System.Guid.Empty ? guid.ToByteArray() : new System.Guid().ToByteArray();

        /// <summary>
        /// Returns a new Guid byte array if the given Guid byte array is empty,
        /// otherwise returns the given Guid.
        /// </summary>
        public static byte[] GetSerializableGuid(this byte[] guid)
        {
            // If the byte array is null, return a new Guid byte array.
            if (guid == null)
                return new System.Guid().ToByteArray();
            
            // If the byte array is empty, return a new Guid byte array.
            if (guid.Length == 0)
                return new System.Guid().ToByteArray();
            
            // If the byte array is not empty, but is not 16 bytes long, throw an exception.
            if (guid.Length != 16)
                throw new System.ArgumentException("Guid byte array must be 16 bytes long.", nameof(guid));

            // If the byte array is not an empty Guid, return a new Guid byte array.
            // Otherwise, return the given Guid byte array.
            System.Guid guidObj = new System.Guid(guid);
            return guidObj != System.Guid.Empty ? guidObj.ToByteArray() : new System.Guid().ToByteArray(); 
        }

        #if UNITY_EDITOR
        /// <summary>
        /// Draws a text string in as a gizmo in world space. This only works in the Unity Editor.
        /// </summary>
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