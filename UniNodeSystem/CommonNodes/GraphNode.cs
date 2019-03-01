using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class GraphNode : UniNode
    {

        public UniGraph Target;
        
        public bool WaitGraph = true;
        
        
        public override string GetName()
        {
            return Target ? Target.name : name;
        }
        
        protected override IEnumerator ExecuteState(IContext context)
        {
            
            if (!WaitGraph)
            {
                yield return base.ExecuteState(context);
            }

            if (Target)
            {
                yield return Target.Execute(context);
            }

            if (WaitGraph)
            {
                yield return base.ExecuteState(context);
            }
            
        }
    }
    
}
