using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    //Extension Methods involving math related to geometry and space. Includes bounds calculation and intersection useful in collision detection.
    public static partial class ExtensionMethods
    {
        

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
        /// Returns a Vector3 position along a parabola as a function of time.
        /// </summary>
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            float arc = -4 * height * t * t + 4 * height * t;
            Vector3 mid = Vector3.Lerp(start, end, t);
            return new Vector3(mid.x, arc + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

    }
}
