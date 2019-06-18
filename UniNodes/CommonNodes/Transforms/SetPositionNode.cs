namespace UniGreenModules.UniNodes.CommonNodes.Transforms
{
    using System.Collections;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Runtime;
    using UnityEngine;

    public class SetPositionNode : UniNode
    {
        
        [SerializeField] private Vector3 targetPosition;

        [SerializeField] private Transform targetPostionObject;

        [SerializeField] private bool setToMyPosition = false;

        [SerializeField] private bool setLocal = false;
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            
            var targetTransform = context.Get<Transform>();

            UpdatePosition(targetTransform);
            
            yield return base.OnExecuteState(context);

        }

        private void UpdatePosition(Transform targetTransform)
        {
            if (!targetTransform)
            {
                return;
            }
            
            if (setToMyPosition)
            {
                SetTargetPosition(targetTransform, transform.position);              
            } 
            else if (targetPostionObject)
            {
                SetTargetPosition(targetTransform, targetPostionObject.transform.position);
            }
            else
            {
                SetTargetPosition(targetTransform, targetPosition);
            }
        }

        private void SetTargetPosition(Transform target, Vector3 targetPosition)
        {
            if (setLocal)
            {
                target.localPosition = targetPosition;
                return;
            }
            target.position = targetPosition;
        }
    }
}
