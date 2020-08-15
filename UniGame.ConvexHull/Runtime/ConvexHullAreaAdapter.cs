namespace UniModules.UniGame.ConvexHull.Runtime
{
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using Core.Runtime.Math;
    using UnityEngine;

    public class ConvexHullAreaAdapter : IConvexHullAreaAdapter
    {
        private readonly IConvexHullAreaCalculator _convexHullAreaCalculator;

        public ConvexHullAreaAdapter(IConvexHullAreaCalculator convexHullAreaCalculator)
        {
            _convexHullAreaCalculator = convexHullAreaCalculator;
        }

        public Vector2[] Adapt(IReadOnlyList<Vector2> convexHull, float minArea)
        {
            var currentArea = _convexHullAreaCalculator.CalculateArea(convexHull);

            if (currentArea < minArea) {
                var difference = Mathf.Sqrt(minArea / currentArea);
                var scaleMatrix = Matrix2D.Scale(new Vector2(difference, difference));
                
                var result = new Vector2[convexHull.Count];
                for (var i = 0; i < result.Length; i++) {
                    result[i] = scaleMatrix.MultiplyPoint(convexHull[i]);
                }

                return result;
            }
            
            return convexHull.ToArray();
        }
    }
}