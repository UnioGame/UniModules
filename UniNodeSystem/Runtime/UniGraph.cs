using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Modules.UniTools.UniNodeSystem;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.UniStateMachine;
using UniModule.UnityTools.UniStateMachine.Extensions;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniModule.UnityTools.UniVisualNodeSystem.Connections;
using UnityEngine;
using UniNodeSystem;
using UniStateMachine.CommonNodes;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniTools.UniRoutine.Runtime;

    public class UniGraph : NodeGraph, IContextState<IEnumerator>
    {
       
        #region private properties

        [NonSerialized] private bool isInitialized = false;

        /// <summary>
        /// graph roots
        /// </summary>
        [NonSerialized] private List<UniRootNode> rootNodes;

        /// <summary>
        /// all child nodes
        /// </summary>
        [NonSerialized] private List<UniGraphNode> allNodes;

        /// <summary>
        /// private graph state executor
        /// </summary>
        private IContextState<IEnumerator> graphState;

        /// <summary>
        /// node executor
        /// </summary>
        private INodeExecutor<IContext> nodeExecutor;
        
        /// <summary>
        /// state local context data
        /// </summary>
        protected IContext localContext;


        #endregion
        
        #region public properties

        public ILifeTime LifeTime => graphState?.LifeTime;

        public bool IsActive => graphState?.IsActive ?? false;
        
        #endregion

        public void Initialize()
        {
            graphState = CreateState();
            
            if (isInitialized)
                return;

            isInitialized = true;

            nodeExecutor = new NodeRoutineExecutor();
            rootNodes = new List<UniRootNode>();

            for (var i = 0; i < nodes.Count; i++) {
                if(nodes[i] is UniRootNode rootNode)
                    rootNodes.Add(rootNode);
            }

            InitializeNodes();
            InitializePortConnections();
        }

        #region context state
        
        public void Release() => Exit();
        
        public IEnumerator Execute(IContext context)
        {
            Initialize();

            ActiveGraphs.Add(this);

            yield return graphState.Execute(context);
        }

        public void Exit()
        {
            graphState?.Exit();
            ActiveGraphs.Remove(this);
        }

        #endregion
        
        #region private methods
        
        private IEnumerator OnExecute(IContext context)
        {
            if (rootNodes == null || rootNodes.Count == 0)
            {
                Debug.LogErrorFormat("Graph root nodes not found");
                yield break;
            }

            var lifeTime = LifeTime;

            for (var i = 0; i < rootNodes.Count; i++)
            {
                var rootNode           = rootNodes[i];
                var rootDisposableItem = rootNode.Execute(context).RunWithSubRoutines();
                lifeTime.AddDispose(rootDisposableItem);
            }

        }

        protected void OnExit(IContext context)
        {
            if (allNodes == null) return;

            for (var i = 0; i < allNodes.Count; i++) {
                var node = allNodes[i];
                node.Exit();
            }
            
            localContext = null;
            graphState = null;
        }

        private IContextState<IEnumerator> CreateState()
        {
            if (graphState != null)
                return graphState;
            
            var stateBehaviour = ClassPool.Spawn<ProxyState>();
            
            stateBehaviour.Initialize(OnExecute, x => localContext = x, OnExit);
            stateBehaviour.LifeTime.AddCleanUpAction(() => ClassPool.Despawn(stateBehaviour));
            
            return stateBehaviour;
        }
        
        private void InitializeNodes()
        {
            allNodes = new List<UniGraphNode>();

            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (!(node is UniGraphNode graphNode))
                {
                    continue;
                }

                graphNode.Initialize();

                allNodes.Add(graphNode);
            }
        }

        /// <summary>
        /// create bindings between portvalues
        /// </summary>
        private void InitializePortConnections()
        {
            for (var i = 0; i < allNodes.Count; i++)
            {
                var node = allNodes[i];
                var values = node.PortValues;

                for (var j = 0; j < values.Count; j++)
                {
                    var value = values[j];
                    var port = node.GetPort(value.name);

                    //take only input ports
                    if (port.direction == PortIO.Output)
                        continue;

                    var connection = node.Input == value
                        ? new InputPortConnection(node, value,nodeExecutor)
                        : new PortValueConnection(value);

                    BindWithOutputs(connection, port);
                }
            }
        }

        private void BindWithOutputs(IContextWriter inputConnection, NodePort port)
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

        private void OnDisable()
        {
            Exit();
        }

        #endregion

    }
}