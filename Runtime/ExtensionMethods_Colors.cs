using UnityEngine;

namespace Buck
{
    // Extension Methods involving UnityEngine.Color and UnityEngine.Gradient
    public static partial class ExtensionMethods
    {
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
    }
}
