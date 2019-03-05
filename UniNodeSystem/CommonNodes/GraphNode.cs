using System.Collections;
using System.Collections.Generic;
using Modules.UniTools.UnityTools.Attributes;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ResourceSystem;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniNodeSystem;
using UniStateMachine.Nodes;
using UnityEngine;

namespace UniStateMachine.CommonNodes
{
    public class GraphNode : UniNode
    {
        #region private properties

        /// <summary>
        /// target node graph
        /// </summary>
        protected UniGraph UniGraph => Graph.Load<UniGraph>();

        #endregion
        
        #region inspector data
        
        /// <summary>
        /// target graph resource
        /// </summary>
        [TargetType(typeof(UniGraph))]
        public ResourceItem Graph;
        
        /// <summary>
        /// Wait target graph execution
        /// </summary>
        public bool WaitGraph = true;

        #endregion
        
        public override string GetName()
        {
            var asset = UniGraph;
            return asset ? asset.name : base.GetName();
        }
        
        protected override IEnumerator ExecuteState(IContext context)
        {

            if (!WaitGraph)
            {
                yield return base.ExecuteState(context);
            }
            
            var targetGraph = UniGraph;
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

            var graphAsset = UniGraph;

            if (graphAsset == null)
                return;
            
            var nodes = graphAsset.nodes;
            foreach (var node in nodes)
            {
                if(!(node is UniNode uniNode))
                    continue;
                
                uniNode.Initialize();
                
                if (node is IGraphPortNode outputNode)
                {
                    RegisterGraphPort(node.GetName(),outputNode.PortValue,outputNode.Direction);
                }

            }

        }

        private void RegisterGraphPort(string portName,UniPortValue value,PortIO direction)
        {
            var portPair = this.UpdatePortValue(portName, direction);
            portPair.value.Add(value);
        }
    }
    
}
