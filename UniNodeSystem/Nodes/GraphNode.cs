namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections;
    using Runtime;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UniCore.Runtime.AsyncOperations;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.Rx.Extensions;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UniTools.UniRoutine.Runtime;

    public class GraphNode : UniNode
    {
#region inspector data

        [Tooltip("should we await target graph before pass data to output or not")]
        [SerializeField]
        public bool awaitGraph = false;

        [Tooltip("Bind subgraph to node lifetime")]
        [SerializeField]
        private bool bindWithLifetime = true;

        /// <summary>
        /// cached addressable asset name
        /// </summary>
        [HideInInspector]
        public string graphName;

        /// <summary>
        /// target graph resource
        /// </summary>
        public AssetReferenceGameObject graphReference;

#endregion

        /// <summary>
        /// graph addressable instance loading handle
        /// </summary>
        public AsyncOperationHandle<GameObject> GraphInstanceHandle => graphReference.LoadAssetAsync();

        public UniGraph graphInstance;

        public override string GetName()
        {
            return string.IsNullOrEmpty(graphName) ? base.GetName() : graphName;
        }


        protected override IEnumerator OnExecuteState(IContext context)
        {
            if (graphInstance == null) {
                //await graph loading
                yield return GraphInstanceHandle.Task.AwaitTask();
                //spawn graph
                graphInstance = GraphInstanceHandle.Result.Spawn<UniGraph>();
            }

            //despawn when execution finished
            var lifeTime = LifeTime;
            //initialize graph            
            graphInstance.Initialize();

            //bind node ports to graph nodes
            BindGraphPorts(graphInstance);

            //if graph depends from node lifetime => despawn on exit
            if (bindWithLifetime) {
                lifeTime.AddCleanUpAction(() => {
                                              graphInstance.Exit();
                                              graphInstance.AssetInstance.Despawn();
                                              graphInstance = null;
                                          });
            }

            if (awaitGraph) {
                yield return graphInstance.Execute(context);
            }
            else {
                graphInstance.Execute(context).RunWithSubRoutines(graphInstance.RoutineType).AddTo(lifeTime);
            }

            yield return base.OnExecuteState(context);
        }

        protected override void OnUpdatePortsCache()
        {
            base.OnUpdatePortsCache();

#if UNITY_EDITOR

            var graphAsset = graphReference.editorAsset as GameObject;
            //if target addressable is empty
            if (!graphAsset)
                return;

            var targetGraph = graphAsset.GetComponent<UniGraph>();
            if (targetGraph == null)
                return;

            targetGraph.Initialize();

            foreach (var port in targetGraph.Ports) {
                var portValue      = this.UpdatePortValue(port.fieldName, port.direction);
                var graphPortValue = targetGraph.GetPortValue(port.fieldName);
                portValue.value.Connect(graphPortValue);
            }

#endif
        }

        private void BindGraphPorts(IUniGraph targetGraph)
        {
            var lifetime = LifeTime;
            foreach (var node in targetGraph.GraphInputs) {
                BindGraphPort(node, lifetime);
            }

            foreach (var node in targetGraph.GraphOuputs) {
                BindGraphPort(node, lifetime);
            }
        }

        private void BindGraphPort(IGraphPortNode graphNode, ILifeTime lifeTime)
        {
            if (graphNode.Visible == false)
                return;

            var portName  = graphNode.ItemName;
            var graphPort = graphNode.PortValue;

            var port = GetPortValue(portName);

            var source = graphNode.Direction == PortIO.Input ? port : graphPort;
            var target = graphNode.Direction == PortIO.Input ? graphPort : port;

            source.Connect(target);
            lifeTime.AddCleanUpAction(() => source.Disconnect(target));
        }
    }
}