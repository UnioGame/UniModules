using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniModule.UnityTools.Common;
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

        [NonSerialized] private bool _isInitialized = false;

        /// <summary>
        /// graph roots
        /// </summary>
        [NonSerialized] private List<UniRootNode> _rootNodes;

        /// <summary>
        /// all child nodes
        /// </summary>
        [NonSerialized] private List<UniGraphNode> _allNodes;

        /// <summary>
        /// private graph state executor
        /// </summary>
        private IContextState<IEnumerator> _graphState;

        /// <summary>
        /// state local context data
        /// </summary>
        protected IContext _contextData;
        

        #endregion
        
        #region public properties

        public ILifeTime LifeTime => _graphState == null ? null : _graphState.LifeTime;


        public bool IsActive => _graphState == null ? false : _graphState.IsActive;
        
        #endregion

        public void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            _rootNodes = nodes.OfType<UniRootNode>().ToList();
            _graphState = CreateState();

            InitializeNodes();
            InitializePortConnections();
        }

        public IEnumerator Execute(IContext context)
        {
            Initialize();

            yield return _graphState.Execute(context);
        }

        public void Exit()
        {
            _graphState?.Exit();
        }

        #region INodeExecutor

        public void Execute(UniGraphNode node, IContext context)
        {
            if (node.IsActive)
                return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STARTED", node);

            var inputValue = node.Input;
            inputValue.Add(context);

            var awaiter = node.Execute(context);
            var disposable = awaiter.RunWithSubRoutines(node.RoutineType);

            //cleanup actions
            var lifeTime = node.LifeTime;
            lifeTime.AddDispose(disposable);
        }

        public void Stop(UniGraphNode node, IContext context)
        {
            //node already stoped
            if (!node.IsActive)
                return;

            StateLogger.LogState($"GRAPH NODE {node.name} : STOPED", node);

            node.Exit();
        }

        #endregion

        #region private

        protected void OnExit(IContext context)
        {
            
            _contextData = null;
            
            if (_allNodes == null) return;

            for (var i = 0; i < _allNodes.Count; i++)
            {
                var node = _allNodes[i];
                node.Exit();
            }

        }

        private IContextState<IEnumerator> CreateState()
        {
            var stateBehaviour = new ProxyState();
            stateBehaviour.Initialize(OnExecute, x => _contextData = x, OnExit);
            return stateBehaviour;
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
                    if (port.direction == PortIO.Output)
                        continue;

                    var connection = node.Input == value
                        ? new InputPortConnection(this, node, value)
                        : new PortValueConnection(value);

                    BindWithOutputs(connection, port);
                }
            }
        }

        private void BindWithOutputs(ITypeDataWriter inputConnection, NodePort port)
        {
            var connections = port.GetConnections();

            for (int i = 0; i < connections.Count; i++)
            {
                var connection = connections[i];
                var connectedNode = connection.node;

                if (!(connectedNode is UniGraphNode node))
                    continue;

                //register connection with target input 
                var connectedValue = node.GetPortValue(connection.fieldName);
                connectedValue.Connect(inputConnection);
            }
        }

        private IEnumerator OnExecute(IContext context)
        {
            if (_rootNodes == null || _rootNodes.Count == 0)
            {
                Debug.LogErrorFormat("Graph root nodes not found");
                yield break;
            }

            var lifeTime = LifeTime;

            for (var i = 0; i < _rootNodes.Count; i++)
            {
                var rootNode = _rootNodes[i];
                var rootDisposableItem = rootNode.Execute(context).RunWithSubRoutines();
                lifeTime.AddDispose(rootDisposableItem);
            }

        }

        #endregion

        public void Release()
        {
            Exit();
        }
    }
}