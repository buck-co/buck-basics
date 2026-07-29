// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// Describes which display aspect ratios a game supports, as bounds on width divided by height.
    /// Bounds rather than a list of ratios, because real resolutions rarely reduce to their marketing
    /// ratio: 1366x768 is 683:384, 2560x1080 is 64:27, and 1920x1200 is 8:5. A minimum of 1.6 admits
    /// 16:10, 16:9, 21:9 and wider, while rejecting 4:3 and 5:4.
    /// Assign the same asset to everything that filters or applies resolutions so they all agree.
    /// </summary>
    [CreateAssetMenu(fileName = "AspectRatioPolicy", menuName = "BUCK/Aspect Ratio Policy")]
    public class AspectRatioPolicy : BaseScriptableObject
    {
        public enum Modes
        {
            Off,     // Every aspect ratio is allowed.
            Minimum, // Reject anything narrower than Min Aspect.
            Maximum, // Reject anything wider than Max Aspect.
            Range    // Reject anything outside Min Aspect and Max Aspect.
        };

        // Keeps a hand-typed bound from rejecting a size that sits on it. 3440x1440 is 2.3888889,
        // so a Max Aspect of 2.389 would reject it without a little slack.
        const float k_epsilon = 0.001f;

        [SerializeField, Tooltip("Which of the bounds below apply. Off allows every aspect ratio, " +
                                 "which is the same as not assigning a policy at all.")]
        Modes m_mode = Modes.Off;
        public Modes Mode => m_mode;

        [SerializeField, Tooltip("Narrowest supported width divided by height. " +
                                 "1.25 is 5:4, 1.333 is 4:3, 1.5 is 3:2, 1.6 is 16:10, 1.778 is 16:9.")]
        float m_minAspect = 1.6f;
        public float MinAspect => m_minAspect;

        [SerializeField, Tooltip("Widest supported width divided by height. " +
                                 "1.778 is 16:9, 2.389 is 21:9, 3.556 is 32:9.")]
        float m_maxAspect = 2.4f;
        public float MaxAspect => m_maxAspect;

        /// <summary>True when this policy will actually reject something.</summary>
        public bool IsActive => m_mode != Modes.Off;

        /// <summary>
        /// True if a size's aspect ratio is supported. Sizes with a non-positive width or height are
        /// never allowed, even when the mode is Off.
        /// </summary>
        public bool IsAllowed(int width, int height)
        {
            if (width <= 0 || height <= 0) return false;
            if (m_mode == Modes.Off) return true;

            float ratio = GetRatio(width, height);

            bool checkMin = m_mode == Modes.Minimum || m_mode == Modes.Range;
            bool checkMax = m_mode == Modes.Maximum || m_mode == Modes.Range;

            if (checkMin && ratio < m_minAspect - k_epsilon) return false;
            if (checkMax && ratio > m_maxAspect + k_epsilon) return false;

            return true;
        }

        public bool IsAllowed(Vector2Int size) => IsAllowed(size.x, size.y);

        /// <summary>
        /// Returns desired if its aspect ratio is already supported. Otherwise returns the largest
        /// allowed candidate that fits inside desired, or the smallest allowed candidate if none fit.
        /// Returns desired unchanged when no candidate is allowed, so callers always get a usable size.
        /// </summary>
        public Vector2Int ResolveNearestAllowed(Vector2Int desired, IEnumerable<Vector2Int> candidates)
        {
            if (IsAllowed(desired) || candidates == null) return desired;

            // Vector2Int.zero is a safe "nothing yet" sentinel because IsAllowed rejects non-positive sizes.
            Vector2Int bestFit = Vector2Int.zero;
            Vector2Int smallest = Vector2Int.zero;

            foreach (var candidate in candidates)
            {
                if (!IsAllowed(candidate)) continue;

                if (IsBetter(candidate, smallest, preferLarger: false))
                    smallest = candidate;

                // Never hand back something larger than the display the caller asked about.
                if (candidate.x > desired.x || candidate.y > desired.y) continue;

                if (IsBetter(candidate, bestFit, preferLarger: true))
                    bestFit = candidate;
            }

            if (bestFit != Vector2Int.zero) return bestFit;
            if (smallest != Vector2Int.zero) return smallest;
            return desired;
        }

        /// <summary>Width divided by height, or 0 for a non-positive size.</summary>
        public static float GetRatio(int width, int height)
            => width > 0 && height > 0 ? (float)width / height : 0f;

        public static float GetRatio(Vector2Int size) => GetRatio(size.x, size.y);

        /// <summary>Configures this policy from code. Intended for tests and editor tooling.</summary>
        public void SetValues(Modes mode, float minAspect, float maxAspect)
        {
            m_mode = mode;
            m_minAspect = minAspect;
            m_maxAspect = maxAspect;
        }

        // Ties break on width so the result never depends on candidate enumeration order.
        static bool IsBetter(Vector2Int candidate, Vector2Int incumbent, bool preferLarger)
        {
            if (incumbent == Vector2Int.zero) return true;

            int candidateArea = Area(candidate);
            int incumbentArea = Area(incumbent);

            if (candidateArea != incumbentArea)
                return preferLarger ? candidateArea > incumbentArea : candidateArea < incumbentArea;

            return candidate.x > incumbent.x;
        }

        static int Area(Vector2Int size) => size.x * size.y;
    }
}
