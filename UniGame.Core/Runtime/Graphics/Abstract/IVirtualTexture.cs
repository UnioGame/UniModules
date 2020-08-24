namespace UniModules.UniGame.Core.Runtime.Graphics.Abstract
{
    using UnityEngine;

    public interface IVirtualTexture
    {
        int Width  { get; }
        int Height { get; }

        int PixelCount { get; }
        
        Color GetPixelColor(float x, float y);
    }
}