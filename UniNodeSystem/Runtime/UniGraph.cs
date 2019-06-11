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
    using UniTools.UniRoutine.Runtime;

    public class UniGraph : NodeGraph, IUniGraph
    {
       
        #region private properties

        /// <summary>
        /// graph roots
        /// </summary>
        [NonSerialized] private List<UniRootNode> rootNodes;

        /// <summary>
        /// all child nodes
        /// </summary>
        [NonSerialized] private List<UniGraphNode> allNodes;

        /// <summary>
        /// node executor
        /// </summary>
        private INodeExecutor<IContext> nodeExecutor;

        #endregion

        public override void Dispose() => Exit();
        
        #region private methods
        
        protected override IEnumerator OnExecuteState(IContext context)
        {
            ActiveGraphs.Add(this);

            var lifeTime = LifeTime;
            for (var i = 0; i < rootNodes.Count; i++)
            {
                rootNodes[i].
                    Execute(context).
                    RunWithSubRoutines().AddTo(lifeTime);
            }

            yield return base.OnExecuteState(context);
        }

        protected override void OnExit(IContext context)
        {
            allNodes.ForEach(x => x.Exit());
            ActiveGraphs.Remove(this);
        }
        
        protected override void OnNodeInitialize()
        {
            nodeExecutor = new NodeRoutineExecutor();

            InitializeNodes();
            InitializePortConnections();
        }

        private void InitializeNodes()
        {
            rootNodes = new List<UniRootNode>();
            allNodes  = new List<UniGraphNode>();

            for (var i = 0; i < nodes.Count; i++) {

                var node = nodes[i];
                
                if (!(node is UniGraphNode graphNode))
                    continue;

                if(node is UniRootNode rootNode)
                    rootNodes.Add(rootNode);

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

        private void OnDisable() => Dispose();

        #endregion

    }
}