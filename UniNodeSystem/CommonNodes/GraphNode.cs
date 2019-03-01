using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ResourceSystem;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class GraphNode : UniNode
    {
        public ResourceItem Graph;
        public bool WaitGraph = true;

        public override string GetName()
        {
            return Graph.HasValue() ? Graph.ItemName : name;
        }
        
        protected override IEnumerator ExecuteState(IContext context)
        {

            if (!WaitGraph)
            {
                yield return base.ExecuteState(context);
            }
            
            var targetGraph = Graph.Load<UniGraph>();
            if (targetGraph)
            {
                yield return targetGraph.Execute(context);
            }

            if (WaitGraph)
            {
                yield return base.ExecuteState(context);
            }
            
        }
    }
    
}
