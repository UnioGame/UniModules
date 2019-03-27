using System.Collections;
using UniModule.UnityTools.Interfaces;
using UnityEngine;

namespace UniStateMachine.CommonNodes.Transforms
{
    public class SetPositionNode : UniNode
    {
        
        [SerializeField] private Vector3 targetPosition;

        [SerializeField] private Transform targetPostionObject;

        [SerializeField] private bool setToMyPosition = false;

        [SerializeField] private bool setLocal = false;
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            yield return base.ExecuteState(context);

            var targetTransform = context.Get<Transform>();
            if (!targetTransform)
                yield break;

            if (setToMyPosition)
            {
                SetTargetPosition(targetTransform, transform.position);
                yield break;
            }
            if (targetPostionObject)
            {
                SetTargetPosition(targetTransform, targetPostionObject.transform.position);
                yield break;
            }
            SetTargetPosition(targetTransform, targetPosition);
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
