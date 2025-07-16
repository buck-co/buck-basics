// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using UnityEngine;

namespace Buck
{
    // Extension Methods involving Floats
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Returns true if a value is between a minimum (inclusive) and maximum (inclusive).
        /// </summary>
        public static bool IsBetween(this float value, float min, float max)
            => value >= min && value <= max;

        /// <summary>
        /// Remaps a value from a minimum and maximum range to another minimum and maximum range.
        /// </summary>
        public static float Remap(this float value, float fromMin, float fromMax, float toMin, float toMax)
            => Mathf.Lerp(toMin, toMax, Mathf.InverseLerp(fromMin, fromMax, value));

        /// <summary>
        /// Remaps a value from a minimum and maximum range to a zero to one range.
        /// </summary>
        public static float Remap01(this float value, float min, float max)
            => Remap(value, min, max, 0, 1);

        /// <summary>
        /// Rounds a float to a specified number of digits after the decimal point.
        /// </summary>
        public static float Round(this float value, int digits)
            => (float)System.Math.Round(value, digits);

        /// <summary>
        /// Rounds a double to a specified number of digits after the decimal point.
        /// </summary>
        public static double Round(this double value, int digits)
            => System.Math.Round(value, digits);

        /// <summary>
        /// Raises a float value by a power and keeps its sign (positive or negative).
        /// </summary>
        public static float SignedPow(float value, float pow)
            => Mathf.Pow(Mathf.Abs(value), pow) * Mathf.Sign(value);
        
        /// <summary>
        /// Returns true if the signs of two floats are the same.
        /// </summary>
        public static bool SameSign(this float a, float b)
            => a * b >= 0.0f;

        /// <summary>
        /// Applies an "ease in" curve to a linear value t.
        /// </summary>
        public static float EaseIn(float t)
            => 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        
        /// <summary>
        /// Applies an "ease out" curve to a linear value t.
        /// </summary>
        public static float EaseOut(float t)
            => Mathf.Sin(t * Mathf.PI * 0.5f);
        
        /// <summary>
        /// Applies a "smoothstep" curve to a linear value t.
        /// </summary>
        public static float Smoothstep(float t)
            => t * t * (3f - 2f * t);
        
        /// <summary>
        /// Ken Perlin's better smooth step with 1st and 2nd order derivatives at x = 0 and 1.
        /// </summary>
        public static float Smootherstep(float from, float to, float x) 
        {
            x = Mathf.Clamp01(x);
            return Mathf.Lerp(from, to, x * x * x * (x * (x * 6 - 15) + 10));
        }

        /// <summary>
        /// Returns the volume of a sphere given radius r.
        /// </summary>
        public static float SphereVolume(float r)
            => (4f/3f)*Mathf.PI*r*r*r;

        /// <summary>
        /// Returns the surface area of a sphere given radius r.
        /// </summary>
        public static float SphereSurfaceArea(float r)
            => 4f * Mathf.PI * r * r;

        /// <summary>
        /// Remaps any value to 0-360 as if it is a positive value in degrees. For example, 362f will return 2f. -10 will return 350f.
        /// </summary>
        public static float Angle360Positive(this float degrees)
        {
            float ret = degrees % 360f;

            if (ret < 0f)
                ret = 360f + ret;

            return ret;
        }
    }
}
