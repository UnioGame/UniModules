using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniModule.UnityTools.DataFlow;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniModule.UnityTools.ProfilerTools;
using UniModule.UnityTools.UniRoutine;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UnityEngine;
using UniNodeSystem;
using UniStateMachine.CommonNodes;

namespace UniStateMachine.Nodes
{
	public class UniGraph : NodeGraph, IContextState<IEnumerator>, INodeExecutor<IContext>
    {
	    #region private properties

	    [NonSerialized]
	    private bool _isInitialized = false;
	    [NonSerialized]
	    private List<IGraphPortNode> _rootNodes;
	    [NonSerialized]
	    private Dictionary<UniPortValue, IContextDataWriter<IContext>> _connections;
	    
	    private List<UniGraphNode> _allNodes;

	    /// <summary>
	    /// private graph state executor
	    /// </summary>
	    private IContextState<IEnumerator> _graphState;
	    
	    /// <summary>
	    /// state local context data
	    /// </summary>
	    protected IContextData<IContext> _contextData;

	    #endregion

	    public void Initialize()
	    {
		    if (_isInitialized)
			    return;
		    
		    _isInitialized = true;
		    
		    _connections = new Dictionary<UniPortValue, IContextDataWriter<IContext>>();
		    _rootNodes = nodes.OfType<IGraphPortNode>().
			    Where(x => x.Direction == PortIO.Input).ToList();
		    
		    var stateBehaviour = new ProxyStateBehaviour();
		    stateBehaviour.Initialize(OnExecute,
			    x => _contextData = x,OnExit);
		    _graphState = stateBehaviour;
			    
		    InitializeNodes();
		    InitializePortConnections();
		    
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

		public override void Dispose()
		{
		    if (_contextData == null)
		        return;

		    var contexts = ClassPool.Spawn<List<IContext>>();
		    contexts.AddRange(_contextData.Contexts);
		    
			foreach (var context in contexts)
			{
				OnExit(context);
				_contextData.RemoveContext(context);
			}
			
        }
		
		
		public void Execute(UniGraphNode node, IContext context)
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

		public void Stop(UniGraphNode node, IContext context)
		{
			//node already active for this context
			if (!node.IsActive(context))
				return;

			StateLogger.LogState($"GRAPH NODE {node.name} : STOPED", node);

			node.Exit(context);

		}

	    
	    #region private

	    protected void OnExit(IContext context)
	    {
		    if (_allNodes == null) return;

		    for (var i = 0; i < _allNodes.Count; i++)
		    {
			    var node = _allNodes[i];
			    node.Exit(context);
		    }
	    }
	    
	    private void InitializeNodes()
	    {
		    _allNodes = new List<UniGraphNode>();

		    for (var i = 0; i < nodes.Count; i++)
		    {
			    var node = nodes[i];
			    if (!(node is UniGraphNode graphNode))
			    {
				    continue;
			    }

			    graphNode.Initialize();

			    _allNodes.Add(graphNode);

		    }
		    
		    
	    }

	    /// <summary>
	    /// create bindings between portvalues
	    /// </summary>
	    private void InitializePortConnections()
	    {

		    for (var i = 0; i < _allNodes.Count; i++)
		    {
			    var node = _allNodes[i];
			    var values = node.PortValues;

			    for (var j = 0; j < values.Count; j++)
			    {
				    var value = values[j];
				    var port = node.GetPort(value.Name);
				    
					//take only input ports
				    if(port.direction == PortIO.Output)
					    continue;

				    var connection = node.Input == value ?
					    new InputPortConnection(this,node,value) : 
					    new PortValueConnection(value);
				    
				    _connections[value] = connection;

				    BindWithOutputs(connection, port);

			    }
			    
		    }
		    
	    }

	    private void BindWithOutputs(IContextDataWriter<IContext> inputConnection,NodePort port)
	    {
		    var connections = port.GetConnections();

		    for (int i = 0; i < connections.Count; i++)
		    {
			    var connection = connections[i];
			    var connectedNode = connection.node;
			    
			    if(!(connectedNode is UniGraphNode node))
				    continue;

			    //register connection with target input 
			    var connectedValue = node.GetPortValue(connection.fieldName);
			    connectedValue.Add(inputConnection);
			    
		    }
		    
	    }
	    
	    private IEnumerator OnExecute(IContext context)
	    {
		    if (_rootNodes == null || _rootNodes.Count == 0)
		    {
			    Debug.LogErrorFormat("Graph root nodes not found");
			    yield break;
		    }

		    var lifeTime = GetLifeTime(context);

		    for (var i = 0; i < _rootNodes.Count; i++)
		    {
			    var rootNode = _rootNodes[i];
			    var rootDisposableItem = rootNode.Execute(context).RunWithSubRoutines();
			    lifeTime.AddDispose(rootDisposableItem);
		    }

	    }

		#endregion
	}
}
