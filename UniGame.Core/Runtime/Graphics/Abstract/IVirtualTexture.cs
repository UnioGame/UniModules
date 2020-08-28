namespace UniModules.UniGame.Core.Runtime.Graphics.Abstract
{
    using UnityEngine;

    public interface IVirtualTexture
    {
        int Width { get; }
        int Height { get; }

        Bounds Bounds { get; }

        Color GetPixelColor(float x, float y);
    }
}