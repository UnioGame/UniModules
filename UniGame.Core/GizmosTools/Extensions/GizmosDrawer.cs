namespace UniGreenModules.UniCore.GizmosTools.Extensions
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    public static class GizmosDrawer
    {
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 from, Vector3 to, float length) {

            var direction = (to - from).normalized;
            Gizmos.DrawLine(from,from + (direction * length));
		
        }
	
        [Conditional("UNITY_EDITOR")]
        public static void DrawDirectionLine(Vector3 from, Vector3 to, float length) {

            var direction = to.normalized;
            Gizmos.DrawLine(from,from + (direction * length));
		
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawDotLine(Vector3 from, Vector3 to, float pointSize) {
		
            Gizmos.DrawSphere(from,pointSize);
            Gizmos.DrawLine(from,to);
            Gizmos.DrawSphere(to,pointSize);
		
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawAndRevertColor(Action action) 
        {
            var gizmoColor = Gizmos.color;

            DrawGizmosWithColor(action, gizmoColor);
		
            Gizmos.color = gizmoColor;
        }
        
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
