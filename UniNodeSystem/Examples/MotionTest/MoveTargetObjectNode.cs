using System.Collections;
using UniModule.UnityTools.Interfaces;
using UniStateMachine;
using UnityEngine;

namespace MotionTest
{
    public class MoveTargetObjectNode : UniNode
    {
       
        public Vector2 _stepRange = new Vector2(0.5f,5f);
        
        protected override IEnumerator ExecuteState(IContext context)
        {

            var target = context.Get<GameObject>();
            target.SetActive(true);
            var transform = target.transform;
            
            var rotationPoint = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f));
            var angle = Random.Range(_stepRange.x,_stepRange.y);   
            
            while (true)
            {
                
                transform.RotateAround(Vector3.zero,rotationPoint,angle);
                
                yield return null;
                
            }
            
            yield return base.ExecuteState(context);
            
        }
    }
}
