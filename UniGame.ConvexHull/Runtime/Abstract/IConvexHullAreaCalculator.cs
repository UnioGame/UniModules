namespace UniModules.UniGame.ConvexHull.Runtime.Abstract
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IConvexHullAreaCalculator
    {
        float CalculateArea(IReadOnlyList<Vector2> points);
    }
}