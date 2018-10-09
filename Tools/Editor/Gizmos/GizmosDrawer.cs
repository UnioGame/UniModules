using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosDrawer
{

	public static void DrawLine(Vector3 from, Vector3 to, float length) {

		var direction = (to - from).normalized;
		Gizmos.DrawLine(from,from + (direction * length));
		
	}
	
	public static void DrawDirectionLine(Vector3 from, Vector3 to, float length) {

		var direction = to.normalized;
		Gizmos.DrawLine(from,from + (direction * length));
		
	}

	public static void DrawDotLine(Vector3 from, Vector3 to, float pointSize) {
		
		Gizmos.DrawSphere(from,pointSize);
		Gizmos.DrawLine(from,to);
		Gizmos.DrawSphere(to,pointSize);
		
	}

	public static void DrawAndRevertColor(Action action) 
	{
		var gizmoColor = Gizmos.color;
		
		action?.Invoke();
		
		Gizmos.color = gizmoColor;
	}
	
}
