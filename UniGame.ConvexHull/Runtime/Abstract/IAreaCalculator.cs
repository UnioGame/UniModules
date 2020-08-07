namespace UniModules.UniGame.ConvexHull.Runtime.Abstract
{
    using UnityEngine;

    public interface IAreaCalculator
    {
        float CalculateArea(Vector2 sceneSize, Vector2 screenSize, float minSizeInPixels);
    }
}