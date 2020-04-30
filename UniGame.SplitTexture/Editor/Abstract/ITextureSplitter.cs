namespace UniModules.UniGame.SplitTexture.Editor.Abstract
{
    using UnityEngine;

    public interface ITextureSplitter
    {
        Texture2D[] SplitTexture(Texture2D source, Vector2Int maxSize);
    }
}