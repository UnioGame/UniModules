namespace UniGreenModules.UniCore.Runtime.Collider
{
    using System.Collections.Generic;
    using UnityEngine;

	public static class ColliderExtension
	{
        public static void ClearPaths(this PolygonCollider2D collider)
        {
            collider.pathCount = 0;
        }

        public static void SetCollider(this PolygonCollider2D collider, Sprite mask)
		{
			var triangles = mask.triangles;
            var vertices = mask.vertices;
            
            var edges = new Dictionary<string, KeyValuePair<int, int>>();
            for (var i = 0; i < triangles.Length; i += 3) {
                for (var k = 0; k < 3; k++) {
                    var point0 = triangles[i + k];
                    var point1 = triangles[i + k + 1 > i + 2 ? i : i + k + 1];
                    var edge = $"{Mathf.Min(point0, point1)}:{Mathf.Max(point0, point1)}";
                    if (edges.ContainsKey(edge)) {
                        edges.Remove(edge);
                    }
                    else {
                        edges.Add(edge, new KeyValuePair<int, int>(point0, point1));
                    }
                }
            }
            
            var lookup = new Dictionary<int, int>();
            foreach (var value in edges.Values) {
                if(!lookup.ContainsKey(value.Key))
                    lookup.Add(value.Key, value.Value);
            }

            collider.pathCount = 0;
            
            var startPoint = 0;
            var nextPoint = startPoint;
            var highestPoint = startPoint;
            var colliderPath = new List<Vector2>();

            while (true) {
                colliderPath.Add(vertices[nextPoint]);
                
                nextPoint = lookup[nextPoint];

                if (nextPoint > highestPoint) {
                    highestPoint = nextPoint;
                }

                if (nextPoint == startPoint) {
                    collider.pathCount++;
                    collider.SetPath(collider.pathCount - 1, colliderPath.ToArray());
                    colliderPath.Clear();

                    if (lookup.ContainsKey(highestPoint + 1)) {
                        startPoint = highestPoint + 1;
                        nextPoint = startPoint;
                        
                        continue;
                    }
                    
                    break;
                }
            }
		}
	}
}
