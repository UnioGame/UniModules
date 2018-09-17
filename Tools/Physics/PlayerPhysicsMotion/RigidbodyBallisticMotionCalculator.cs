using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsBallisticsMotion {

    public class RigidbodyBallisticMotionCalculator {

        private readonly Vector3 _gravity;
        private readonly Vector3 _halfGravity;

        public RigidbodyBallisticMotionCalculator(Vector3 gravity) {
            _gravity = gravity;
            _halfGravity = _gravity * 0.5f;
        }


        public Vector3 GetPosition(BallisticsMotionData data) {

            var deltaTime = data.Time;
            var deltaTimeSqr = Mathf.Pow(deltaTime, 2);

            var gravityEffect = _halfGravity * deltaTimeSqr;
            var forceAtTime = data.Force * deltaTime;
            var result = forceAtTime + gravityEffect;

            return data.InitialPosition + result;
            
        }

    }
}
