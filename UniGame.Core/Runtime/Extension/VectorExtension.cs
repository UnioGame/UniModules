namespace UniGreenModules.UniCore.Runtime.Extension
{
    using UnityEngine;

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

        public static Vector2Int Clamp(this Vector2Int vector, Vector2Int range)
        {
            var result = new Vector2Int();
            result.x = Mathf.Clamp(vector.x, range.x, range.y);
            result.y = Mathf.Clamp(vector.y, range.x, range.y);
            return result;
        }

        public static Vector3 Reflect(this Vector3 point, Vector3 source, Vector3 reflectionLine) {

            var dot = Vector3.Dot(point, reflectionLine);
            var projection = reflectionLine * dot;
            var projectionPoint = projection - point;
            return point + 2 * projectionPoint;

        }
        
        public static float AbsAngle(this Vector3 pivot, Vector3 firstPoint, Vector3 secondPoint)
        {
            Vector2 origin = firstPoint - pivot;
            Vector2 to = secondPoint - pivot;
            var angle = Vector2.Angle(origin, to);
            angle = Mathf.Abs(angle);
            return angle;
        }
    }
}
