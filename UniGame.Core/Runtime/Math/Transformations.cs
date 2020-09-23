namespace UniModules.UniGame.Core.Runtime.Math
{
    using UnityEngine;

    public static class Transformations
    {
        public static Vector2 TransformWorldToRectPoint(this Vector2 worldPoint, Bounds bounds, Rect rect)
        {
            var boundsX = Mathf.Abs(worldPoint.x - bounds.min.x);
            var boundsY = Mathf.Abs(bounds.max.y - worldPoint.y);

            var ratioX = boundsX / bounds.size.x;
            var ratioY = boundsY / bounds.size.y;

            var rectX = rect.min.x + ratioX * rect.width;
            var rectY = rect.min.y + ratioY * rect.height;
            
            return new Vector2(rectX, rectY);
        }
        
        public static Rect TransformBoundsToRelativeRect(Bounds a, Bounds b, float aWidth, float aHeight, float bWidth, float bHeight)
        {
            var firstX = Mathf.Abs(b.min.x - a.min.x);
            var firstY = Mathf.Abs(b.max.y - a.max.y);

            var ratioX = firstX / a.size.x;
            var ratioY = firstY / a.size.y;

            var x = aWidth * ratioX;
            var y = aHeight * ratioY;
            
            return new Rect(x, y, bWidth, bHeight);
        }
        
        public static void ExpandRect(ref Rect rect, float expandMultiplier, float widthLimit, float heightLimit)
        {
            var widthOffset = rect.width * expandMultiplier;
            var heightOffset = rect.height * expandMultiplier;
            
            rect.xMin = Mathf.Clamp(rect.xMin - widthOffset * 0.5f, 0.0f, widthLimit);
            rect.yMin = Mathf.Clamp(rect.yMin - heightOffset * 0.5f, 0.0f, heightLimit);
            rect.xMax = Mathf.Clamp(rect.xMax + widthOffset * 0.5f, 0.0f, widthLimit);
            rect.yMax = Mathf.Clamp(rect.yMax + widthOffset * 0.5f, 0.0f, heightLimit);
        }
    }
}