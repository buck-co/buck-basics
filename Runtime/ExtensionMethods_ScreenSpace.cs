// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    // Extension Methods relying on Cameras to convert objects like RectTransforms or Bounds in world space into positions on screen or vice versa
    public static partial class ExtensionMethods
    {
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
            => IsVisibleFrom(renderer.bounds, camera);

        //*****[Rects, Bounds, RectTransforms, and Screen Space]*****
        /// <summary>
        /// Converts a RectTransform into a Bounds
        /// </summary>
        public static Bounds GetRectTransformBounds(this RectTransform rectTransform)
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
        public static Rect GetBoundsRectangle(this Bounds bounds)
            => Rect.MinMaxRect(bounds.min.x, bounds.min.y, bounds.max.x, bounds.max.y);

        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(this Renderer renderer, Camera camera = null)
            => GetScreenRectangle(renderer.bounds, camera);
        
        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(this RectTransform rectTransform, Camera camera = null)
            => GetScreenRectangle(GetRectTransformBounds(rectTransform), camera);

        /// <summary>
        /// Returns a Rect that represents the screen space position of the bounding box around a world space object.
        /// Credit to YouTube user quill18creates in this video https://www.youtube.com/watch?v=2Tgqr1_ajqE
        /// </summary>
        public static Rect GetScreenRectangle(this Bounds bounds, Camera camera = null)
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
    }
}
