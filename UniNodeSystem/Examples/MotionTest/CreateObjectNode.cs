using System.Collections;
using UniModule.UnityTools.ActorEntityModel;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniStateMachine;
using UniStateMachine.Nodes;
using UnityEngine;
using XNode;

namespace MotionTest
{
    public class CreateObjectNode : UniNode
    {
        private UniPortValue _objectValuePort;
        
        public int Count;

        public int MaxRange;

        public GameObject Target;
        
        protected override IEnumerator ExecuteState(IContext context)
        {

            for (var i = 0; i < Count; i++)
            {
                var objectContext = new EntityObject();
                
                var x = Random.Range(-MaxRange, MaxRange);
                var y = Random.Range(-MaxRange, MaxRange);
                var targetPosition = new Vector3(x,y,0);
                var target = Instantiate(Target,targetPosition,Quaternion.identity);
                
                objectContext.Add(target);
                
                _objectValuePort.UpdateValue(objectContext,objectContext);
                
            }

            yield return base.ExecuteState(context);
            
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();
            _objectValuePort = this.UpdatePortValue("ObjectOutput", PortIO.Output).value;
        }
    }
}
