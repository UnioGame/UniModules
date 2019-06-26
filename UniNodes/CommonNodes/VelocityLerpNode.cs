namespace UniGreenModules.UniNodes.CommonNodes
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UniNodeSystem.Runtime.NodeData;
    using UnityEngine;

    public class VelocityLerpNode : UniNode
    {
        private Vector3 _normalizedDirection;
        private float _maxSqrMagnitude;
        private float _minSqrMagnitude;
        
        #region inspector

        public ForceNodeData ForceData = new ForceNodeData();
        
        #endregion

        protected override void OnInitialize(IContext localContext)
        {
            base.OnInitialize(localContext);

            var direction = ForceData.Direction;
            
            _normalizedDirection = direction.normalized;
        }

        protected override IEnumerator OnExecuteState(IContext context)
        {
            yield return base.OnExecute(context);

            var activeTime = 0f;
            var startTime = Time.realtimeSinceStartup;
            
            while (true)
            {                              
                yield return null;

                var rigidbody = context.Get<Rigidbody>();

                if (!rigidbody)
                {
                    continue;
                }

                activeTime = Time.realtimeSinceStartup - startTime;

                var progress = Mathf.Approximately(ForceData.LerpTime,0f) ? 1 :
                    activeTime / ForceData.LerpTime;

                var velocity = Mathf.Lerp(ForceData.MinForce, ForceData.MaxForce, progress);

                var velocityVector = _normalizedDirection * velocity;
                
                rigidbody.AddForce(velocityVector,ForceData.ForceMode);

            }
            
        }
    }
}
