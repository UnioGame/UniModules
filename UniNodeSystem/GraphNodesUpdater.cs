using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UnityEngine;
using UniNodeSystem;

namespace UniStateMachine.Nodes
{
    public class GraphNodesUpdater : IGraphNodesUpdater
    {
        private readonly INodeExecutor<IContext> _executor;
        private readonly List<IContext> _updateContext;
        private readonly Dictionary<IContext, UniNode> _cacheNodes;

        public GraphNodesUpdater(INodeExecutor<IContext> executor)
        {
            _updateContext = new List<IContext>();
            _cacheNodes = new Dictionary<IContext, UniNode>();
            
            _executor = executor;
        }
        
        public void UpdateNode(UniNode node)
        {

            GameProfiler.BeginSample("GraphUpdater_GetData");
            
            var input = node.GetPort(UniNode.InputPortName);
            
            var inputPortValue = node.GetPortValue(input);

            GameProfiler.EndSample();
            
            GameProfiler.BeginSample("GraphUpdater_UpdateNodes");
            
            UpdateNodeState(node,inputPortValue);

            GameProfiler.EndSample();
            
        }

        private void UpdateNodeState(UniNode node,UniPortValue input)
        {
            _updateContext.AddRange(node.Contexts);
            _updateContext.AddRange(input.Contexts);
            
            for (int i = 0; i < _updateContext.Count; i++)
            {

                var context = _updateContext[i];
				
                if (!input.HasContext(context))
                {
                    _executor.Stop(node, context);
                    continue;
                }
								
                GameProfiler.BeginSample("Graph_UpdateNodeState");

                UpdateNode(node,context);
				
                GameProfiler.EndSample();
				
            }	         
            
            _updateContext.Clear();
        }
        
        private void UpdateNode(UniNode node, IContext context)
        {
							
            if (node.Validate(context))
            {
                if (node.IsActive(context))
                    return;
                _executor.Execute(node, context);
            }
            else
            {
                _executor.Stop(node, context);
            }
        
        }
        
    }
}
