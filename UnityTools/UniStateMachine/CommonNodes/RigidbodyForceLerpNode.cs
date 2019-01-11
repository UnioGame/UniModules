using System.Collections;
using Assets.Tools.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class RigidbodyVelocityLerpNode : UniNode
    {
        #region inspector

        public Vector3 MaxVelocity;

        public Vector3 MinVelocity;

        public float LerpTime = 1f;

        #endregion

        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var activeTime = 0f;
            var startTime = Time.realtimeSinceStartup;
            
            while (IsActive(context))
            {                              
                yield return null;

                var rigidbody = context.Get<Rigidbody>();

                if(!rigidbody)
                    continue;

                activeTime = Time.realtimeSinceStartup - startTime;

                var progress = Mathf.Approximately(LerpTime,0f) ? 1 :
                    activeTime / LerpTime;

                Vector3.Lerp(MinVelocity, MaxVelocity, progress);
            }
            
        }
    }
}
