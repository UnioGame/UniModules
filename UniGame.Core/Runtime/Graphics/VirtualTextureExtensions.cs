namespace UniModules.UniGame.Core.Runtime.Graphics
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using UnityEngine;

    public static class VirtualTextureExtensions
    {
        public static Color GetAverageColor(this IVirtualTexture virtualTexture, Rect rect, float alpha = 0.5f)
        {
            var colors = new List<Color>();
            
            for (var w = rect.xMin; w < rect.xMin + rect.width; w++) {
                for (var h = rect.yMin; h < rect.yMin + rect.height; h++) {
                    var color = virtualTexture.GetPixelColor(w, h);
                    if(color.a < alpha)
                        continue;

                    colors.Add(color);
                }
            }

            var colorCount = colors.Count;
            if (colorCount % 2 == 0) {
                var firstColor = colors[colorCount / 2];
                var secondColor = colors[colorCount / 2 - 1];

                return (firstColor + secondColor) * 0.5f;
            }

            return colors[Mathf.FloorToInt(colorCount * 0.5f)];
        }

        public static float GetContrast(this IVirtualTexture virtualTexture, Rect rect)
        {
            var pixelsContrast = new List<float>();

            for (var w = rect.xMin; w < rect.xMin + rect.width; w++) {
                for (var h = rect.yMin; h < rect.yMin + rect.height; h++) {
                    var color = virtualTexture.GetPixelColor(w, h);
                    if(Mathf.Approximately(color.a, 0.0f))
                        continue;

                    var pixelContrast = color.GetLuminance() * color.a;
                    
                    pixelsContrast.Add(pixelContrast);
                }
            }

            pixelsContrast = pixelsContrast.OrderBy(x => x).ToList();
            var pixelsCount = pixelsContrast.Count;
            
            if (pixelsCount % 2 == 0) {
                return (pixelsContrast[pixelsCount / 2] + pixelsContrast[pixelsCount / 2 - 1]) * 0.5f;
            }

            return pixelsContrast[Mathf.FloorToInt(pixelsCount * 0.5f)];
        }
        
        public static int GetOpaquePixelCount(this IVirtualTexture virtualTexture, Rect rect)
        {
            var opaquePixelCount = 0;
            
            for (var w = rect.xMin; w < rect.xMin + rect.width; w++) {
                for (var h = rect.yMin; h < rect.yMin + rect.height; h++) {
                    var color = virtualTexture.GetPixelColor(w, h);
                    if(Mathf.Approximately(color.a, 0.0f))
                        continue;

                    opaquePixelCount++;
                }
            }

            return opaquePixelCount;
        }
    }
}