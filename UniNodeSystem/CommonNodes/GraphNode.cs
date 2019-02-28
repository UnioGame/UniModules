using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class GraphNode : AssetNode<UniNodesGraph>
    {

        public bool WaitGraph = true;
        
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
