namespace UniModules.UniGame.Core.Runtime.Graphics
{
    using UnityEngine;

    public static class ColorExtensions
    {
        public static Color AdditiveSum(this Color source, Color other)
        {
            if(source.a == 0.0f && other.a == 0.0f)
                return Color.clear;
            if (source.a == 0.0f)
                return other;
            if (other.a == 0.0f)
                return source;

            var alpha = 1.0f - (1.0f - other.a) * (1.0f - source.a);
            var red   = other.r * other.a / alpha + source.r * source.a * (1.0f - other.a) / alpha;
            var green = other.g * other.a / alpha + source.g * source.a * (1.0f - other.a) / alpha;
            var blue  = other.b * other.a / alpha + source.b * source.a * (1.0f - other.a) / alpha;
            
            return new Color(red, green, blue, alpha);
        }
    }
}