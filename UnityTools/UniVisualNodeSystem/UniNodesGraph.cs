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
using UnityEngine;
using XNode;

namespace UniStateMachine.Nodes
{
    [CreateAssetMenu(fileName = "UniGraph", menuName = "UniStateMachine/UniGraph")]
	public class UniNodesGraph : NodeGraph, IContextState<IEnumerator>, INodeExecutor<IContext>
    {
	    #region private properties

	    [NonSerialized]
	    private bool _isInitialized = false;
	    [NonSerialized]
	    private List<UniRootNode> _rootNodes;
	    
	    private List<UniNode> _uniNodes;
	    private List<UniGraphNode> _allNodes;
	    private IGraphNodesUpdater _updater;

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
		    _updater = new GraphNodesUpdater(this);
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
		
		
		public void Execute(UniNode node, IContext context)
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

		public void Stop(UniNode node, IContext context)
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
	    
	    protected virtual IEnumerator UpdateGraph(IContext context)
	    {

		    while (IsActive(context))
		    {

			    for (var i = 0; i < _uniNodes.Count; i++)
			    {
				    var node = _uniNodes[i];
				    _updater.UpdateNode(node);
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

		    var disposable = UpdateGraph(context).RunWithSubRoutines();
		    lifeTime.AddDispose(disposable);

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
