using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.ProfilerTools;
using Assets.Tools.UnityTools.StateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UniStateMachine.NodeEditor.NodeExtensions;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [CreateAssetMenu(fileName = "UniGraph", menuName = "UniStateMachine/UniGraph")]
	public class UniNodesGraph : NodeGraph, IContextState<IEnumerator>
    {
	    #region private properties

	    [NonSerialized]
	    private List<UniRootNode> _rootNodes;
	    
	    private Dictionary<UniNode, List<UniPortValue>> _activeNodes;
	    
	    private IContextState<IEnumerator> _graphState;
	    /// <summary>
	    /// state local context data
	    /// </summary>
	    protected IContextData<IContext> _contextData;

	    protected IContextState<IEnumerator> GraphState
	    {
		    get
		    {
			    if (_graphState == null)
			    {
				    var state = new ProxyStateBehaviour();
				    state.Initialize(OnExecute,OnInitialize,null);
				    _graphState = state;
			    }

			    return _graphState;
		    }
	    }

	    public List<UniRootNode> RootNodes
	    {
		    get
		    {
			    if (_rootNodes == null)
			    {
				    _rootNodes = nodes.OfType<UniRootNode>().ToList();
			    }
			    return _rootNodes;
		    }   
	    }

	    #endregion
	    
	    public bool IsActive(IContext context)
		{

		    return _contextData!=null &&
		           _contextData.HasContext(context);
        
		}

		public ILifeTime GetLifeTime(IContext context)
		{
			return GraphState.GetLifeTime(context);
		}

		public IEnumerator Execute(IContext context)
		{

			yield return GraphState.Execute(context);

		}

		public void Exit(IContext context)
		{
			
			GraphState.Exit(context);

        }

		public void Dispose()
		{
		    if (_contextData == null)
		        return;
			foreach (var context in _contextData.Contexts.ToList())
			{
			    Exit(context);
			}
        }
	    
	    #region private
	    	    
	    protected virtual IEnumerator UpdateGraph(UniGraphData graphContext)
	    {

		    while (true)
		    {

			    for (var i = 0; i < nodes.Count; i++)
			    {
				    if(!(nodes[i] is UniNode node))
				    {
					    continue;
				    }
				    UpdateNode(node, graphContext);
			    }
				
			    yield return null;
		    }
			
	    }
	    
	    private void OnInitialize(IContextData<IContext> contextData)
	    {
		    _contextData = contextData;
	    }
	    
	    private IEnumerator OnExecute(IContext context)
	    {
		    var roots = RootNodes;
		    if (roots == null || roots.Count == 0)
		    {
			    Debug.LogErrorFormat("Graph root nodes not found");
			    yield break;
		    }

		    var lifeTime = GetLifeTime(context);
		    var graphData = ClassPool.Spawn<UniGraphData>();
		    graphData.Initialize(context);
			
		    _contextData.AddValue(context, graphData);

		    foreach (var rootNode in roots)
		    {
			    var dispose = rootNode.Execute(context).RunWithSubRoutines();
			    lifeTime.AddCleanUpAction(() => rootNode.Exit(context));
			    lifeTime.AddDispose(dispose);
		    }
		    
		    var disposable = UpdateGraph(graphData).RunWithSubRoutines();

		    lifeTime.AddDispose(disposable);
		    lifeTime.AddCleanUpAction(() => graphData?.Despawn());

	    }

        private void UpdateNode(UniNode node, UniGraphData graphContext)
		{
            GameProfiler.BeginSample("UpdateNodes");

			var input = node.InputPort;
			var connections = input.GetConnections();
            var connectedContexts = ClassPool.Spawn<Dictionary<IContext, NodePort>>();
		    var removedItems = ClassPool.Spawn<List<IContext>>();

		    //group contexts data from all connections
		    for (int i = 0; i < connections.Count; i++)
		    {
			    var connection = connections[i];
			    if (!(connection.node is UniGraphNode graphNode))
					continue;

			    var portValue = graphNode.GetPortValue(connection);
			    if(portValue == null) continue;

			    var contexts = portValue.Contexts;
			    foreach (var context in contexts)
			    {
				    connectedContexts[context] = connection;
			    }
		    }
		    
		    var activeContexts = graphContext[node];
		    if (activeContexts != null)
		    {
		        foreach (var context in activeContexts.Keys)
		        {
			        if(!connectedContexts.ContainsKey(context))
						removedItems.Add(context);
                }
		    }

            for (var i = 0; i < removedItems.Count; i++)
            {
	            var context = removedItems[i];
	            StopNode(node, context, graphContext);
            }

            foreach (var connection in connectedContexts)
		    {
                UpdateNode(node, connection.Key, graphContext);
		    }

            connections.DespawnCollection();
		    connectedContexts.DespawnDictionary();
		    removedItems.DespawnCollection();

		    GameProfiler.EndSample();
        }

		private void UpdateNode(UniNode node, IContext context, UniGraphData graphContext)
		{

            if (node.Validate(context))
		    {
		        LaunchNode(node, context, graphContext);
		    }
		    else
		    {
		        StopNode(node, context, graphContext);
		    }
        
		}

		private void LaunchNode(UniNode node, IContext context, UniGraphData graphContext)
		{
		    if (node.IsActive(context))
		        return;

		    var data = graphContext.Get(node, context);

		    StateLogger.LogState($"GRAPH NODE {node.name} : STARTED", node);

            //if node already was started, but restart option disabled
            //when pass target node
            if (data!=null)
			{
			    data.Activate(node,context);
			    return;
			}

            var item = ClassPool.Spawn<NodeContextData>();
            item.Activate(node,context);
		    graphContext.Add(node,context, item);

		}
		
		private void StopNode(UniNode node, IContext context, UniGraphData graphContext)
		{
            //node already active for this context
		    if (!node.IsActive(context))
		        return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STOPED", node);

            var output = node.Output.Value;
            var outputContext = output.Get<IContext>(context);
            graphContext.Release(node, context);

		    if (outputContext == null)
		        return;

            //stop all connected nodes
		    var outputNodes = node.OutputPort.GetConnectedNodes<UniNode>();
            for (int i = 0; i < outputNodes.Count; i++)
		    {
		        var item = outputNodes[i];
		        StopNode(item, outputContext, graphContext);
		    }
            outputNodes.DespawnCollection();

		}

		#endregion
	}
}
