namespace UniModules.UniGame.Core.Runtime.Graphics
{
    using Abstract;
    using UnityEngine;

    public class VirtualTexture : IVirtualTexture
    {
        private const float PixelsPerUnit = 100;
        
        private readonly Texture2D _texture;
        private readonly Rect      _rect;

        public int    Width  { get; }
        public int    Height { get; }
        public Bounds Bounds { get; }

        public VirtualTexture(Texture2D texture)
        {
            _texture = texture;

            Width    = texture.width;
            Height   = texture.height;
            
            _rect = new Rect(0.0f, 0.0f, texture.width, texture.height);
            Bounds = new Bounds(new Vector3(0.0f, 0.0f), new Vector3(Width/PixelsPerUnit, Height/PixelsPerUnit));
        }

        public VirtualTexture(Sprite sprite)
        {
            _texture = sprite.texture;

            Width  = (int) sprite.textureRect.width;
            Height = (int) sprite.textureRect.height;

            _rect  = sprite.textureRect;
            Bounds = sprite.bounds;
        }

        public Color GetPixelColor(float x, float y)
        {
            var realX = _rect.xMin + x;
            var realY = _rect.yMin + y;
            
            return _texture.GetPixel((int)realX, (int)realY);
        }
    }
}