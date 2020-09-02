namespace UniModules.UniGame.Core.Runtime.Graphics
{
    using System;
    using UnityEngine;

    public static class ColorExtensions
    {
        private const float SRgbDitheringThreshold = 0.03928f;
        
        public static Color AdditiveSum(this Color source, Color other)
        {
            if(Math.Abs(source.a) < float.Epsilon && Math.Abs(other.a) < float.Epsilon)
                return Color.clear;
            if (Math.Abs(source.a) < float.Epsilon)
                return other;
            if (Math.Abs(other.a) < float.Epsilon)
                return source;

            var alpha = 1.0f - (1.0f - other.a) * (1.0f - source.a);
            var red   = other.r * other.a / alpha + source.r * source.a * (1.0f - other.a) / alpha;
            var green = other.g * other.a / alpha + source.g * source.a * (1.0f - other.a) / alpha;
            var blue  = other.b * other.a / alpha + source.b * source.a * (1.0f - other.a) / alpha;
            
            return new Color(red, green, blue, alpha);
        }
        
        public static float GetLuminance(this Color pixelColor)
        {
            return NonlinearTransformation(pixelColor.r) * 0.2126f + NonlinearTransformation(pixelColor.g) * 0.7152f + NonlinearTransformation(pixelColor.b) * 0.0722f;
        }
        
        public static float NonlinearTransformation(float channelValue)
        {
            if (channelValue <= SRgbDitheringThreshold) {
                return channelValue / 12.92f;
            }

            return Mathf.Pow((channelValue + 0.055f) / 1.055f, 2.4f);
        }
    }
}