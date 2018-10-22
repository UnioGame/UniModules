using UnityEngine;

namespace Assets.Tools.UnityTools.Extension
{
    public static class VectorExtension  {

        public static bool IsLeft(this Vector3 a, Vector3 b)
        {

            return (-a.x * b.y + a.y * b.x) < 0;

        }
        
        public static bool IsLeft(this Vector3 from,Vector3 to,Vector3 direction)
        {
            var source =  to - from;
            return source.IsLeft(direction);
        }
        
        public static float Cross2D(this Vector3 a, Vector3 b)
        {
            return (-a.x * b.y + a.y * b.x);
        }

        public static Vector3 Rotate902D(this Vector3 source) {
            
            return new Vector3(-source.y,source.x,source.z);
            
        }

        public static Vector3 Reflect(this Vector3 point, Vector3 source, Vector3 reflectionLine) {

            var dot = Vector3.Dot(point, reflectionLine);
            var projection = reflectionLine * dot;
            var projectionPoint = projection - point;
            return point + 2 * projectionPoint;

        }
    }
}
