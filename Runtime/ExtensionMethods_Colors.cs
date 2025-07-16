// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    // Extension Methods involving UnityEngine.Color and UnityEngine.Gradient
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Adds a tint to a Color.
        /// </summary>
        public static Color Tint(this Color value, float tint)
            => new Color(Mathf.Clamp01(value.r + (tint * (1 - value.r))),
                         Mathf.Clamp01(value.g + (tint * (1 - value.g))),
                         Mathf.Clamp01(value.b + (tint * (1 - value.b))),
                         value.a);

        /// <summary>
        /// Sets the alpha of a Color.
        /// </summary>
        public static Color SetAlpha(this Color value, float alpha)
            => new Color(value.r, value.g, value.b, alpha);

        /// <summary>
        /// Multiples each R, G, B, and A value of one Color by another Color's respective R,G,B, and A values and clamps them 0-1
        /// </summary>
        public static Color Multiply(this Color a, Color b, bool ignoreAlpha = true)
            => new Color (Mathf.Clamp01(a.r * b.r),
                          Mathf.Clamp01(a.g * b.g),
                          Mathf.Clamp01(a.b * b.b),
                          ignoreAlpha ? a.a : Mathf.Clamp01(a.a * b.a));
        
        /// <summary>
        /// Adds each R, G, B, and A value of one Color by another Color's respective R,G,B, and A values and clamps them 0-1
        /// </summary>
        public static Color Add(this Color a, Color b, bool ignoreAlpha = true)
            => new Color (Mathf.Clamp01(a.r + b.r),
                          Mathf.Clamp01(a.g + b.g),
                          Mathf.Clamp01(a.b + b.b),
                          ignoreAlpha ? a.a : Mathf.Clamp01(a.a + b.a));

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

        public static void Tint(this Graphic graphic, float tint)
            => graphic.color = graphic.color.Tint(tint);
        
        public static void SetAlpha(this Graphic graphic, float alpha)
            => graphic.color = graphic.color.SetAlpha(alpha);
        
        public static void Multiply(this Graphic graphic, Color multiplier, bool ignoreAlpha = true)
            => graphic.color = graphic.color.Multiply(multiplier, ignoreAlpha);
        
        public static void Add(this Graphic graphic, Color multiplier, bool ignoreAlpha = true)
            => graphic.color = graphic.color.Add(multiplier, ignoreAlpha);
        
        public static void Tint(this SpriteRenderer spriteRenderer, float tint)
            => spriteRenderer.color = spriteRenderer.color.Tint(tint);
        
        public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
            => spriteRenderer.color = spriteRenderer.color.SetAlpha(alpha);
        
        public static void Multiply(this SpriteRenderer spriteRenderer, Color multiplier, bool ignoreAlpha = true)
            => spriteRenderer.color = spriteRenderer.color.Multiply(multiplier, ignoreAlpha);
        
        public static void Add(this SpriteRenderer spriteRenderer, Color multiplier, bool ignoreAlpha = true)
            => spriteRenderer.color = spriteRenderer.color.Add(multiplier, ignoreAlpha);
        
        public static void Tint(this Material material, float tint)
            => material.color = material.color.Tint(tint);
        
        public static void SetAlpha(this Material material, float alpha)
            => material.color = material.color.SetAlpha(alpha);
        
        public static void Multiply(this Material material, Color multiplier, bool ignoreAlpha = true)
            => material.color = material.color.Multiply(multiplier, ignoreAlpha);
        
        public static void Add(this Material material, Color multiplier, bool ignoreAlpha = true)
            => material.color = material.color.Add(multiplier, ignoreAlpha);
    }
}
