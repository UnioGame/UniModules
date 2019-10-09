namespace UniGreenModules.UniCore.GizmosTools.Extensions
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    public static class GizmosDrawer
    {

        [Conditional("UNITY_EDITOR")]
        public static void DrawGizmos(Action drawer)
        {
            DrawGizmosWithColor(drawer, Gizmos.color);
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawGizmosWithColor(Action drawer, Color targetColor)
        {
            var color = Gizmos.color;

            Gizmos.color = targetColor;
            drawer?.Invoke();
            
            Gizmos.color = color;
        }
        
    }
}
