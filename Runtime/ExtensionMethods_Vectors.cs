using UnityEngine;

namespace Buck
{
    // Extension Methods involving Vectors
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Finds the Manhattan Distance between a Vector2Int a, and a Vector2Int b. The manhattan distance is the sum of the horizontal and vertical distance betwen two points. The metaphor being that you are travelling along the blocks of the square grid of Manhattan.
        /// </summary>
        public static int ManhattanDistance(Vector2Int a, Vector2Int b)
            => ManhattanDistance(new Vector3Int(a.x, a.y, 0), new Vector3Int(b.x, b.y, 0));
        
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

        //*****[VECTOR CONVERSION METHODS]*****
        
        //=======[Vector2]========

        /// <summary>
        /// Reconstructs a Vector2 as a Vector2Int rounding each value to the nearest int
        /// </summary>
        public static Vector2Int ToVector2Int(this Vector2 v2)
            => new Vector2Int(Mathf.RoundToInt(v2.x), Mathf.RoundToInt(v2.y));
        
        /// <summary>
        /// Reconstructs a Vector3 as a Vector2Int rounding the x and y value to the nearest int and dropping the z axis
        /// </summary>
        public static Vector2Int ToVector2Int(this Vector3 v3)
            => new Vector2Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y));

        /// <summary>
        /// Reconstructs a Vector2Int as a Vector2
        /// </summary>
        public static Vector2 ToVector2(this Vector2Int v2Int)
            => new Vector2(v2Int.x, v2Int.y);

        //=======[Vector3]========

        /// <summary>
        /// Reconstructs a Vector3 as a Vector3Int rounding each value to the nearest int
        /// </summary>
        public static Vector3Int ToVector3Int(this Vector3 v3)
            => new Vector3Int(Mathf.RoundToInt(v3.x), Mathf.RoundToInt(v3.y), Mathf.RoundToInt(v3.z));
                
        /// <summary>
        /// Reconstructs a Vector2Int as a Vector3, casting x and y to floats and setting z to 0f
        /// </summary>
        public static Vector3 ToVector3(this Vector2Int v2Int)
            => new Vector3(v2Int.x, v2Int.y, 0f);

        /// <summary>
        /// Reconstructs a Vector3Int as a Vector3
        /// </summary>
        public static Vector3 ToVector3(this Vector3Int v3Int)
            => new Vector3(v3Int.x, v3Int.y, v3Int.z);
    }
}
