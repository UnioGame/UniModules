namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Runtime;
    using Runtime.Connections;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
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
        [NonSerialized] private List<IGraphPortNode> inputs = new List<IGraphPortNode>();
        
        /// <summary>
        /// graph outputs
        /// </summary>
        [NonSerialized] private List<IGraphPortNode> outputs = new List<IGraphPortNode>();

        /// <summary>
        /// all child nodes
        /// </summary>
        [NonSerialized] private List<IUniNode> allNodes = new List<IUniNode>();

        /// <summary>
        /// node executor
        /// </summary>
        [NonSerialized] private INodeExecutor<IContext> nodeExecutor = new NodeRoutineExecutor();

        #endregion

        public GameObject AssetInstance => gameObject;

        public IReadOnlyList<IGraphPortNode> GraphOuputs => outputs;
        
        public IReadOnlyList<IGraphPortNode> GraphInputs => inputs;
        
        public override void Dispose() => Exit();
        
        #region private methods
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            //mark graph as active
            ActiveGraphs.Add(this);

            var lifeTime = LifeTime;
            
            for (var i = 0; i < inputs.Count; i++)
            {
                //execute every node with active graph context
                inputs[i].Execute(context).
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
            base.OnUpdatePortsCache();
            
            allNodes.Clear();
            cancelationNodes.Clear();
            inputs.Clear();
            outputs.Clear();
            
            for (var i = 0; i < nodes.Count; i++) {

                var node = nodes[i];
                
                //skip all not unigraph nodes
                if (!(node is UniNode uniNode))
                    continue;

                uniNode.Initialize();
                
                UpdatePortNode(uniNode);

                //stop graph execution, if cancelation node output triggered
                if (uniNode is IGraphCancelationNode) {
                    uniNode.Output.Receive<IContext>().
                        Subscribe(x => Exit()).
                        AddTo(this);
                }

                allNodes.Add(uniNode);
            }
        }

        private void UpdatePortNode(IUniNode uniNode)
        {
            //register input/output nodes
            if (!(uniNode is IGraphPortNode graphPortNode)) {
                return;
            }

            var container = graphPortNode.Direction == PortIO.Input ? 
                inputs : outputs;
            container.Add(graphPortNode);
                    
            //add graph ports for exists port nodes
            var port = this.UpdatePortValue(uniNode.ItemName, graphPortNode.Direction);
            //get node input
            var input = uniNode.Input;           
            //bind graph port to target node input
            port.value.Connect(input);
            
        }
        
        protected override void OnNodeInitialize()
        {
            for (var i = 0; i < allNodes.Count; i++)
            {
                var node   = allNodes[i];
                var values = node.PortValues;

                for (var j = 0; j < values.Count; j++)
                {
                    var value = values[j];
                    var port  = node.GetPort(value.ItemName);

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

        private void OnValidate()
        {
            Initialize();
        }
        
        #endregion

    }
}