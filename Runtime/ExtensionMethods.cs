using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    // Miscellaneous ExtensionMethods that didn't fit in any specific category.
    // Note that ExtensionMethods is defined as a partial and is spread across multiple scripts (see Runtime/ExtensionMethods_Vectors.cs, for example)
    public static partial class ExtensionMethods
    {
        
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
        public static void SetVisible(this CanvasGroup canvasGroup, bool on, bool effectAlpha = true, bool effectInteractable = true, bool effectBlocksRaycasts = true)
        {
            if (effectAlpha)
                canvasGroup.alpha = (on)? 1f: 0f;
            if (effectInteractable)
                canvasGroup.interactable = on;
            if (effectBlocksRaycasts)
                canvasGroup.blocksRaycasts = on;
        }

        /// <summary>
        /// Sets the given Guid byte array to a new Guid byte array if it is null, empty, or an empty Guid.
        /// </summary>
        public static byte[] GetSerializableGuid(ref byte[] guidBytes)
        {
            // If the byte array is null, return a new Guid byte array.
            if (guidBytes == null)
            {
                Debug.Log("Guid byte array is null. Generating a new Guid.");
                guidBytes = System.Guid.NewGuid().ToByteArray();
            }
            
            // If the byte array is empty, return a new Guid byte array.
            if (guidBytes.Length == 0)
            {
                Debug.Log("Guid byte array is empty. Generating a new Guid.");
                guidBytes = System.Guid.NewGuid().ToByteArray();
            }
            
            // If the byte array is not empty, but is not 16 bytes long, throw an exception.
            if (guidBytes.Length != 16)
                throw new System.ArgumentException("Guid byte array must be 16 bytes long.");

            // If the byte array is not an empty Guid, return a new Guid byte array.
            // Otherwise, return the given Guid byte array.
            System.Guid guidObj = new System.Guid(guidBytes);

            if (guidObj == System.Guid.Empty)
            {
                Debug.Log("Guid is empty. Generating a new Guid.");
                guidBytes = System.Guid.NewGuid().ToByteArray();
            }
            
            return guidBytes;
        }
    }
}
