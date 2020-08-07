namespace UniModules.UniGame.ConvexHull.Runtime
{
    using System.Collections.Generic;
    using Abstract;
    using UnityEngine;

    public class ConvexHullAreaCalculator : IConvexHullAreaCalculator
    {
        /// <summary>
        /// Calculate area of a simple polygon whose vertices are described by their Cartesian coordinates in the plane.
        /// https://en.wikipedia.org/wiki/Shoelace_formula
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public float CalculateArea(IReadOnlyList<Vector2> points)
        {
            var area = 0.0f;

            for (int i = points.Count - 1, q = 0; q < points.Count; i = q++) {
                var iPointValue = points[i];
                var qPointValue = points[q];

                area += iPointValue.x * qPointValue.y - qPointValue.x * iPointValue.y;
            }

            return area * 0.5f;
        }
    }
}