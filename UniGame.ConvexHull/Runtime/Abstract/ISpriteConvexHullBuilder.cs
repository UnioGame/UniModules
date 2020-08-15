namespace UniModules.UniGame.ConvexHull.Runtime.Abstract
{
    using UnityEngine;

    public interface ISpriteConvexHullBuilder
    {
        Vector2[] Build(Sprite source);
    }
}