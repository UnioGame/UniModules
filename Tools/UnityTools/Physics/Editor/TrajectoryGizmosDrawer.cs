using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PhysicsBallisticsMotion {

    public class TrajectoryGizmosDrawer {

        [DrawGizmo(GizmoType.Active)]
        public static void DrawGizmoForMyScript(BallisticsMotion motion, GizmoType gizmoType) {

            var color = Gizmos.color;

            var motionPosition = motion.transform.position;
            var position = Input.mousePosition;
            position.z = 10;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(motionPosition, motionPosition + (motion.transform.up * 5));
            Gizmos.DrawLine(motionPosition, motionPosition + (motion.transform.right * 5));

            var worldPosition = Camera.main.ScreenToWorldPoint(position);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(motionPosition, motionPosition + motion.Data.Force);

            var points = motion.Trajectory;
            if (points == null) return;

            Gizmos.color = Color.blue;
            for (var i = 0; i < points.Count; i++) {
                Gizmos.DrawSphere(points[i], 0.2f);
            }

            Gizmos.color = color;
        }
    }
}