namespace UniModules.UniGame.ConvexHull.Runtime
{
    using Abstract;
    using UnityEngine;

    public class MinAreaCalculator : IAreaCalculator
    {
        public float CalculateArea(Vector2 sceneSize, Vector2 screenSize, float minSizeInPixels)
        {
            var width = sceneSize.x / screenSize.x;
            var height = sceneSize.y / screenSize.y;

            var minWidth = width * minSizeInPixels;
            var minHeight = height * minSizeInPixels;

            return minWidth * minHeight;
        }
    }
}