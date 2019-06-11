
namespace UniStateMachine.SubGraph
{
    using System.Collections;
    using System.Collections.Generic;
    using UniGreenModules.UniCore.Runtime.Attributes;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniModule.UnityTools.Interfaces;
    using UniModule.UnityTools.ResourceSystem;
    using UniModule.UnityTools.UniStateMachine.Extensions;
    using UniNodeSystem;
    using UniStateMachine.Nodes;
    using UnityEngine;
    using UniStateMachine.CommonNodes;

    public class GraphNode : UniNode
    {
        private UniGraph _nodeGraph;
        
        #region inspector data
        
        /// <summary>
        /// target graph resource
        /// </summary>
        [TargetType(typeof(UniGraph))]
        public ResourceItem Graph;

        #endregion
        
        public override string GetName()
        {
            var asset = GetGraph();
            return asset ? asset.name : base.GetName();
        }
        
        public UniGraph GetGraph()
        {
            if (_nodeGraph)
                return _nodeGraph;
            
            var resourceGraph = Graph.Load<UniGraph>();
            if (resourceGraph == null)
                return null;
            
            _nodeGraph = resourceGraph;
            
            if (Application.isPlaying)
            {
                var resourceInstance = Instantiate(resourceGraph.gameObject,transform);
                _nodeGraph = resourceInstance.GetComponent<UniGraph>();
            }

            return _nodeGraph;
        }
        
        protected override IEnumerator OnExecuteState(IContext context)
        {

            yield return base.OnExecuteState(context);
            
            var targetGraph = GetGraph();
            if (targetGraph)
            {
                yield return targetGraph.Execute(context);
            }

        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

            var graphAsset = GetGraph();

            if (graphAsset == null)
                return;
            
            var nodes = graphAsset.nodes;
            foreach (var node in nodes)
            {
                if(!(node is UniNode uniNode))
                    continue;
                
                uniNode.Initialize();
                
                if (node is IGraphPortNode graphNode)
                {
                    var nodeName = node.GetName();
                    RegisterGraphPort(nodeName,graphNode.PortValue,graphNode.Direction);
                }

            }

        }

        private void RegisterGraphPort(string portName,UniPortValue value,PortIO direction)
        {
            var portPair = this.UpdatePortValue(portName, direction);
            
            //register node portvalue to inner graph node
            if (direction == PortIO.Input)
            {
                portPair.value.Connect(value);
            }
            else
            {
                value.Connect(portPair.value);
            }
            
            
        }

    }
    
}
