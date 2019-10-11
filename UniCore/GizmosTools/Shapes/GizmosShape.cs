namespace UniGreenModules.UniCore.GizmosTools.Shapes
{
    using System.Diagnostics;
    using UnityEngine;

    public static class GizmosShape
    {
        [Conditional("UNITY_EDITOR")]
        public static void DrawBox(Vector3 center, float width, float height)
        {
            var   zeroPoint = center;
            float w         = 0;
            float h         = 0;
        
            w = width / 2f;
            h = height / 2f;
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z + h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z + h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z - h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z + h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z + h));
        
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawElipse(Vector3 center, float w, float h)
        {
            float theta   = 0;
            var   x       = w * Mathf.Cos(theta);
            var   y       = h * Mathf.Sin(theta);
            var   pos     = center + new Vector3(x, 0, y);
            var   newPos  = pos;
            var   lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x      = w * Mathf.Cos(theta);
                y      = h * Mathf.Sin(theta);
                newPos = center + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }

            Gizmos.DrawLine(pos, lastPos);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRadius(Vector3 center, float radius)
        {
            DrawElipse(center, radius, radius);
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void DrawCircle(Vector3 position, float radius, Color color, bool useAlpha = false)
        {
            radius = Mathf.Abs(radius);
            if (Mathf.Approximately(radius, 0)) return;

            var zeroPoint    = position;

            var r = radius + 0.05f;
            var a = 1.1f;

            color.a = a;
            
            for (var i = 0; i < 10; i++)
            {
                r -= 0.05f;
                a = useAlpha ? a - 0.1f : a;
                float theta   = 0;
                var   x       = r * Mathf.Cos(theta);
                var   y       = r * Mathf.Sin(theta);
                var   pos     = zeroPoint + new Vector3(x, 0, y);
                var   newPos  = pos;
                var   lastPos = pos;
                for (theta = 0.15f; theta < Mathf.PI * 2; theta += 0.15f)
                {
                    x            = r * Mathf.Cos(theta);
                    y            = r * Mathf.Sin(theta);
                    newPos       = zeroPoint + new Vector3(x, 0, y);
                    color.a = a;
                    Gizmos.color = color;
                    Gizmos.DrawLine(pos, newPos);
                    pos = newPos;
                }
                
                color.a = a;
                Gizmos.color = color;
                Gizmos.DrawLine(pos, lastPos);
            }
        }

    }
}
