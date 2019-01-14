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

	    private List<UniNode> _uniNodes;
	    private List<UniGraphNode> _allNodes;
	    
	    [NonSerialized]
	    private List<UniRootNode> _rootNodes;

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
				    state.Initialize(OnExecute,OnInitialize,OnExit);
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
		    
			foreach (var context in _contextData.Contexts)
			{
				OnExit(context);
			}
        }
	    
	    #region private

	    protected void OnExit(IContext context)
	    {
		    if (_allNodes == null) return;

		    for (int i = 0; i < _allNodes.Count; i++)
		    {
			    var node = _allNodes[i];
			    node.Exit(context);
		    }
	    }
	    
	    protected virtual IEnumerator UpdateGraph(IContext context)
	    {

		    while (IsActive(context))
		    {

			    for (var i = 0; i < _uniNodes.Count; i++)
			    {
				    var node = _uniNodes[i];
				    UpdateNode(node);
			    }
				
			    yield return null;
		    }
			
	    }
	    
	    private void OnInitialize(IContextData<IContext> contextData)
	    {
		    _uniNodes = new List<UniNode>();
		    _allNodes = new List<UniGraphNode>();
		    foreach (var node in nodes)
		    {
			    if (!(node is UniGraphNode graphNode))
			    {
				    continue;
			    }
			    _allNodes.Add(graphNode);
			    if(!(graphNode is UniNode uniNode))
			    {
				    continue;
			    }
			    _uniNodes.Add(uniNode);
		    }
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

		    foreach (var rootNode in roots)
		    {
			    var rootDisposableItem = rootNode.Execute(context).RunWithSubRoutines();
			    lifeTime.AddDispose(rootDisposableItem);
		    }
		    
		    var disposable = UpdateGraph(context).RunWithSubRoutines();
		    lifeTime.AddDispose(disposable);

	    }

        private void UpdateNode(UniNode node)
		{
			
            GameProfiler.BeginSample("UpdateNodes");

			var input = node.GetPort(UniNode.InputPortName);
			
			var connections = input.GetConnections();
            var connectedContexts = ClassPool.Spawn<Dictionary<IContext, NodePort>>();
		    var removedItems = ClassPool.Spawn<List<IContext>>();

		    //group contexts data from all connections
		    for (var i = 0; i < connections.Count; i++)
		    {
			    var connection = connections[i];
			    if (!(connection.node is UniGraphNode graphNode))
					continue;

			    var portValue = graphNode.GetPortValue(connection);
			    if(portValue == null) continue;

			    var contexts = portValue.Contexts;
			    foreach (var context in contexts)
			    {
				    var contextValue = portValue.Get<IContext>(context);
				    if(contextValue == null)
					    continue;
				    
				    connectedContexts[context] = connection;
			    }
		    }
		    
		    var activeContexts = node.Input.Contexts;
		    foreach (var context in activeContexts)
		    {
			    if(!connectedContexts.ContainsKey(context))
				    removedItems.Add(context);
		    }
		    
            for (var i = 0; i < removedItems.Count; i++)
            {
	            var context = removedItems[i];
	            StopNode(node, context);
            }

            foreach (var connection in connectedContexts)
		    {
                UpdateNode(node, connection.Key);
		    }

            connections.DespawnCollection();
		    connectedContexts.DespawnDictionary();
		    removedItems.DespawnCollection();

		    GameProfiler.EndSample();
        }

		private void UpdateNode(UniNode node, IContext context)
		{

            if (node.Validate(context))
		    {
		        LaunchNode(node, context);
		    }
		    else
		    {
		        StopNode(node, context);
		    }
        
		}

		private void LaunchNode(UniNode node, IContext context)
		{
		    if (node.IsActive(context))
		        return;

		    StateLogger.LogState($"GRAPH NODE {node.name} : STARTED", node);

		    var inputValue = node.Input;
		    inputValue.UpdateValue(context,context);
            
		    var awaiter = node.Execute(context);
		    var disposable = awaiter.RunWithSubRoutines(node.RoutineType);
            
		    //cleanup actions
		    var lifeTime = node.GetLifeTime(context);
		    lifeTime.AddDispose(disposable);

		}

		private void UpdateInputsValues(UniNode node, IContext context)
		{
			var values = node.PortValues;
			for (int i = 0; i < values.Count; i++)
			{
				var value = values[i];
				
			}
		}
		
		private void StopNode(UniNode node, IContext context)
		{
            //node already active for this context
		    if (!node.IsActive(context))
		        return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STOPED", node);

            node.Exit(context);

		}

		#endregion
	}
}
