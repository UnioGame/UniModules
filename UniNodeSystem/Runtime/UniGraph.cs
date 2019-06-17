namespace UniGreenModules.UniNodeSystem.Runtime
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Connections;
    using Interfaces;
    using Runtime;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.Rx.Extensions;
    using UniRx;
    using UnityEngine;
    using UniTools.UniRoutine.Runtime;

    public class UniGraph : NodeGraph, IUniGraph
    {
        
        #region private properties

        /// <summary>
        /// graph cancelation
        /// </summary>
        [NonSerialized] private List<IGraphCancelationNode> cancelationNodes = new List<IGraphCancelationNode>();
        
        /// <summary>
        /// graph inputs
        /// </summary>
        [NonSerialized] private List<IGraphPortNode> inputNodes = new List<IGraphPortNode>();
        
        /// <summary>
        /// graph outputs
        /// </summary>
        [NonSerialized] private List<IGraphPortNode> outputNodes = new List<IGraphPortNode>();

        /// <summary>
        /// all child nodes
        /// </summary>
        [NonSerialized] private List<IUniNode> allNodes = new List<IUniNode>();

        /// <summary>
        /// node executor
        /// </summary>
        [NonSerialized] private INodeExecutor<IContext> nodeExecutor = new NodeRoutineExecutor();

        #endregion

        public GameObject AssetInstance => this.gameObject;

        public IReadOnlyList<IGraphPortNode> GraphOuputs => outputNodes;
        
        public IReadOnlyList<IGraphPortNode> GraphInputs => inputNodes;
        
        public override void Dispose() => Exit();
        
        #region private methods
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            //mark graph as active
            ActiveGraphs.Add(this);

            var lifeTime = LifeTime;
            
            for (var i = 0; i < inputNodes.Count; i++)
            {
                //execute every node with active graph context
                inputNodes[i].Execute(context).
                    RunWithSubRoutines().
                    AddTo(lifeTime);//subscribe on current lifetime
            }

            //base execution flow
            yield return base.OnExecuteState(context);
        }

        protected override void OnExit(IContext context)
        {
            //stop all nodes
            allNodes.ForEach(x => x.Exit());

            //remove from active graphs
            ActiveGraphs.Remove(this);
        }
        
        protected override void OnUpdatePortsCache()
        {
            allNodes.Clear();
            cancelationNodes.Clear();
            inputNodes.Clear();
            outputNodes.Clear();
            
            InitializeNodes();
        }

        protected override void OnNodeInitialize()
        {
            InitializePortConnections();
        }

        private void InitializeNodes()
        {
            allNodes.Clear();

            for (var i = 0; i < nodes.Count; i++) {

                var node = nodes[i];
                
                //skip all not unigraph nodes
                if (!(node is UniNode uniNode))
                    continue;

                //register input/output nodes
                if (uniNode is IGraphPortNode graphPortNode) {
                    var container = graphPortNode.Direction == PortIO.Input ? 
                        inputNodes : outputNodes;
                    container.Add(graphPortNode);
                }

                //stop graph execution, if cancelation node triggered
                if (uniNode is IGraphCancelationNode) {
                    uniNode.Output.
                        Receive<IContext>().
                        Subscribe(x => Exit()).
                        AddTo(LifeTime);
                }

                uniNode.Initialize();
                
                allNodes.Add(uniNode);
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
                    var port = node.GetPort(value.ItemName);

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

                if (!(connectedNode is UniNode node))
                    continue;

                //register connection with target input 
                var connectedValue = node.GetPortValue(connection.fieldName);
                connectedValue.Connect(inputConnection);
            }
        }

        private void OnDisable() => Dispose();

        #endregion

    }
}