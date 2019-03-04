using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UnityTools.Attributes;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ResourceSystem;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class GraphNode : UniNode
    {
        #region private properties

        /// <summary>
        /// graph output node cache
        /// </summary>
        private List<GraphOuputNode> _graphOutputs = new List<GraphOuputNode>();

        /// <summary>
        /// graph input nodes cache
        /// </summary>
        private List<GraphInputNode> _graphInputs = new List<GraphInputNode>();
        
        #endregion
        
        #region inspector data
        
        [TargetType(typeof(UniGraph))]
        public ResourceItem Graph;
        
        public bool WaitGraph = true;

        #endregion
        
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

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();
            
            _graphOutputs = new List<GraphOuputNode>();
            _graphInputs = new List<GraphInputNode>();
            
        }
    }
    
}
