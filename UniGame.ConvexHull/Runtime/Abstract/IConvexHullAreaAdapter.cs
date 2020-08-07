namespace UniModules.UniGame.ConvexHull.Runtime.Abstract
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IConvexHullAreaAdapter
    {
        Vector2[] Adapt(IReadOnlyList<Vector2> convexHull, float minArea);
    }
}