using System.Collections;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    public class GraphNodesUpdater : IGraphNodesUpdater
    {
        private readonly INodeExecutor<IContext> _executor;
        private readonly List<IContext> _updateContext;
        private readonly Dictionary<UniPortValue, NodePort> _activeNodesPorts;

        public GraphNodesUpdater(INodeExecutor<IContext> executor)
        {
            _updateContext = new List<IContext>();
            _activeNodesPorts = new Dictionary<UniPortValue, NodePort>();
            
            _executor = executor;
        }
        
        public void UpdateNode(UniNode node)
        {

            GameProfiler.BeginSample("UpdateNodes");

            var input = node.GetPort(UniNode.InputPortName);
            var inputPortValue = node.GetPortValue(input);
			
            UpdatePortValue(inputPortValue,input);

            _updateContext.AddRange(inputPortValue.Contexts);
            _updateContext.AddRange(node.Contexts);

            UpdateNodeState(node,inputPortValue,_updateContext);

            _updateContext.Clear();
            _activeNodesPorts.Clear();
            
            GameProfiler.EndSample();
        }

        private void UpdateNodeState(UniNode node,IContextData<IContext> input,List<IContext> contexts)
        {
            
            for (int i = 0; i < contexts.Count; i++)
            {

                var context = contexts[i];
				
                if (!input.HasContext(context))
                {
                    _executor.Stop(node, context);
                    continue;
                }
								
                GameProfiler.BeginSample("Graph_UpdateNodeState");

                UpdateNode(node,context);

                if (node.IsActive(context))
                {
                    var values = node.PortValues;
                    for (var j = 0; j < values.Count; j++)
                    {
                        var portValue = values[j];
                        var port = node.GetPort(portValue.Name);

                        UpdatePortValue(portValue, port);
                    }
                }
				
                GameProfiler.EndSample();
				
            }	

        }
        
        private void UpdateNode(UniNode node, IContext context)
        {

            if (node.Validate(context))
            {
                _executor.Execute(node, context);
            }
            else
            {
                _executor.Execute(node, context);
            }
        
        }
        
        
        private void UpdatePortValue(UniPortValue portValue,NodePort nodePort)
        {           		    
            //if port value already updated => skip port
            if (_activeNodesPorts.ContainsKey(portValue))
                return;
                
            if (nodePort.direction == PortIO.Output)
                return;

            //cleanup port value
            portValue.Release();

            var connections = nodePort.GetConnections();

            //copy values from connected ports to input
            for (var i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                var connectedNode = connection.node;
			    
                if(!(connectedNode is UniGraphNode uniNode)) continue;

                var value = uniNode.GetPortValue(connection.fieldName);
                if(value.Count == 0)
                    continue;
			    
                value?.CopyTo(portValue);
            }
		    
            connections.DespawnCollection();

            _activeNodesPorts[portValue] = nodePort;
            
        }
        
        
    }
}
