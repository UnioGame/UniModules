using System;
using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class RigidbodyVelocityLerpNode : UniNode
    {
        [NonSerialized]
        private Vector3 _normalizedDirection;

        private float _maxSqrMagnitude;
        private float _minSqrMagnitude;
        
        #region inspector

        public Vector3 Direction;
        
        public float MaxVelocity;

        public float MinVelocity;

        public float LerpTime = 1f;

        
        /// <summary>
        /// test velocity work only for 1 context
        /// </summary>
        public Vector3 Velocity;
        
        #endregion

        protected override void OnInitialize(IContextData<IContext> localContext)
        {
            base.OnInitialize(localContext);

            _normalizedDirection = Direction.normalized;
        }

        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var activeTime = 0f;
            var startTime = Time.realtimeSinceStartup;
            
            while (IsActive(context))
            {                              
                yield return null;

                var rigidbody = context.Get<Rigidbody>();

                if (!rigidbody)
                {
                    continue;
                }

                activeTime = Time.realtimeSinceStartup - startTime;

                var progress = Mathf.Approximately(LerpTime,0f) ? 1 :
                    activeTime / LerpTime;

                var velocity = Mathf.Lerp(MinVelocity, MaxVelocity, progress);

                var velocityVector = _normalizedDirection * velocity;
                
                rigidbody.AddForce(velocityVector,ForceMode.VelocityChange);
                
                Velocity = rigidbody.velocity;

            }
            
        }
    }
}
