namespace UniModules.UniGame.ConvexHull.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Abstract;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class SpriteConvexHullBuilder : ISpriteConvexHullBuilder
    {
        /// <summary>
        /// Build convex hull for sprite vertices.
        /// Here gift wrapping algorithm is used (it's also known as Jarvis march).
        /// https://en.wikipedia.org/wiki/Gift_wrapping_algorithm
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public Vector2[] Build(Sprite source)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            if(source.vertices.Length < 3)
                throw new InvalidOperationException("The source must have at least 3 vertices!");
            if (source.vertices.Length == 3)
                return source.vertices;

            return BuildConvexHull(source.vertices);
        }

        private Vector2[] BuildConvexHull(IReadOnlyList<Vector2> vertices)
        {
            var result = new List<Vector2>();
            var points = vertices.ToList();

            var startPoint = points[0];
            for (var i = 1; i < points.Count; i++) {
                var tempPoint = points[i];

                if (tempPoint.x < startPoint.x || Mathf.Approximately(tempPoint.x, startPoint.x) && tempPoint.y < startPoint.y) {
                    startPoint = tempPoint;
                }
            }
            
            result.Add(startPoint);
            points.Remove(startPoint);

            var currentPoint = result[0];
            var collinearPoints = new List<Vector2>();
            var counter = 0;

            while (true) {
                if(counter == 2) {
                    points.Add(result[0]);
                }
                
                var nextPoint = points[Random.Range(0, points.Count)];

                foreach (var point in points) {
                    if(point.Equals(nextPoint))
                        continue;

                    var relation = IsPointLeftOfLine(currentPoint, nextPoint, point);
                    if (relation < float.Epsilon && relation > -float.Epsilon) {
                        collinearPoints.Add(point);
                    }
                    else if(relation < 0.0f) {
                        nextPoint = point;
                        collinearPoints.Clear();
                    }
                }

                if (collinearPoints.Count > 0) {
                    collinearPoints.Add(nextPoint);
                    collinearPoints = collinearPoints.OrderBy(GetOrderKeySelector(currentPoint)).ToList();
                    
                    result.AddRange(collinearPoints);

                    currentPoint = collinearPoints[collinearPoints.Count - 1];

                    foreach (var point in collinearPoints) {
                        points.Remove(point);
                    }
                    
                    collinearPoints.Clear();
                }
                else {
                    result.Add(nextPoint);
                    points.Remove(nextPoint);
                    currentPoint = nextPoint;
                }

                if (currentPoint.Equals(result[0])) {
                    result.RemoveAt(result.Count - 1);
                    break;
                }

                counter++;
            }

            return result.ToArray();
        }

        private Func<Vector2, float> GetOrderKeySelector(Vector2 currentPoint)
        {
            return x=>Vector2.SqrMagnitude(x - currentPoint);
        }

        private float IsPointLeftOfLine(Vector2 a, Vector2 b, Vector2 point)
        {
            return (a.x - point.x) * (b.y - point.y) - (a.y - point.y) * (b.x - point.x);
        }
    }
}