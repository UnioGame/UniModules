namespace UniGame.Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class RectTransformUtils
    {
        public static readonly Vector2 Center      = new Vector2(.5f, .5f);
        public static readonly Vector2 Left        = new Vector2(0f, .5f);
        public static readonly Vector2 Right       = new Vector2(1f, .5f);
        public static readonly Vector2 Top         = new Vector2(.5f, 1f);
        public static readonly Vector2 Bottom      = new Vector2(.5f, 0f);
        public static readonly Vector2 TopLeft     = new Vector2(0f, 1f);
        public static readonly Vector2 TopRight    = new Vector2(1f, 1f);
        public static readonly Vector2 BottomLeft  = new Vector2(0f, 0f);
        public static readonly Vector2 BottomRight = new Vector2(1f, 0f);

        public static IEnumerable<(string, Vector2)> GetAnchorPresets()
        {
            return new List<(string, Vector2)>
            {
                ("Center", new Vector2(.5f, .5f)),
                ("Left", new Vector2(0f, .5f)),
                ("Right", new Vector2(1f, .5f)),
                ("Top", new Vector2(.5f, 1f)),
                ("Bottom", new Vector2(.5f, 0f)),
                ("TopLeft", new Vector2(0f, 1f)),
                ("TopRight", new Vector2(1f, 1f)),
                ("BottomLeft", new Vector2(0f, 0f)),
                ("BottomRight", new Vector2(1f, 0f))
            };
        }

        public static void ResetAnchors(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = Center;
            rectTransform.anchorMax = Center;
            rectTransform.pivot     = Center;
        }
    }
}