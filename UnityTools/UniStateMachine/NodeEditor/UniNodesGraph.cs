using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.ProfilerTools;
using Assets.Tools.UnityTools.StateMachine;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using Assets.Tools.UnityTools.UniRoutine;
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [CreateAssetMenu(fileName = "UniGraph", menuName = "UniStateMachine/UniGraph")]
	public class UniNodesGraph : NodeGraph, IContextState<IEnumerator>
    {
	    #region private properties

	    [NonSerialized]
	    private bool _isInitialized = false;

	    [NonSerialized]
	    private List<UniRootNode> _rootNodes;
	    
	    private List<UniNode> _uniNodes;
	    private List<UniGraphNode> _allNodes;

	    private IContextState<IEnumerator> _graphState;
	    /// <summary>
	    /// state local context data
	    /// </summary>
	    protected IContextData<IContext> _contextData;

	    public IReadOnlyList<UniRootNode> RootNodes => _rootNodes;

	    #endregion

	    public void Initialize()
	    {
		    if (_isInitialized)
			    return;
		    
		    _isInitialized = true;
		    _rootNodes = nodes.OfType<UniRootNode>().ToList();
		    _graphState = GetBehaviour();
		    
		    InitializeNodes();
		    
	    }
	    
	    public bool IsActive(IContext context)
		{

		    return _contextData!=null &&
		           _contextData.HasContext(context);
        
		}

		public ILifeTime GetLifeTime(IContext context)
		{
			return _graphState.GetLifeTime(context);
		}

		public IEnumerator Execute(IContext context)
		{
			Initialize();
			
			yield return _graphState.Execute(context);

		}

		public void Exit(IContext context)
		{
			_graphState.Exit(context);
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
	    
	    private void InitializeNodes()
	    {
		    _uniNodes = new List<UniNode>();
		    _allNodes = new List<UniGraphNode>();
		    
		    foreach (var node in nodes)
		    {
			    if (!(node is UniGraphNode graphNode))
			    {
				    continue;
			    }
			    
			    graphNode.Initialize();
			    
			    _allNodes.Add(graphNode);
			    if(!(graphNode is UniNode uniNode))
			    {
				    continue;
			    }
			    _uniNodes.Add(uniNode);
			    
		    }
		    
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

	    private UniPortValue UpdatePortValue(UniGraphNode node,NodePort nodePort)
	    {

		    if (nodePort.direction == NodePort.IO.Output)
			    return null;
		    
		    var portValue = node.GetPortValue(nodePort.fieldName);
		    if(portValue == null)
			    return null;
		    
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

		    return portValue;
	    }

        private void UpdateNode(UniNode node)
		{
			
            GameProfiler.BeginSample("UpdateNodes");

			var input = node.GetPort(UniNode.InputPortName);
			var value = UpdatePortValue(node,input);

			var contexts = ClassPool.Spawn<List<IContext>>();
			contexts.AddRange(value.Contexts);
			contexts.AddRange(node.Contexts);

			for (int i = 0; i < contexts.Count; i++)
			{
				
				var context = contexts[i];
				
				if (!value.HasContext(context))
				{
					StopNode(node, context);
					continue;
				}
				
				UpdateNode(node,context);

				if (node.IsActive(context))
				{
					var values = node.PortValues;
					for (var j = 0; j < values.Count; j++)
					{
						var portValue = values[j];
						var port = node.GetPort(portValue.Name);
						UpdatePortValue(node, port);
					}
				}
				
			}
			

			contexts.DespawnCollection();

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

		protected virtual IContextState<IEnumerator> GetBehaviour()
		{
			var state = new ProxyStateBehaviour();
			state.Initialize(OnExecute,x => 
				_contextData = x,OnExit);
			return state;
		}

		#endregion
	}
}
