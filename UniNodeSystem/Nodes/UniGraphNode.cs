using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections.Generic;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using Sirenix.Utilities;
    using UniCore.Runtime.ObjectPool;

    public class UniGraphNode : UniNode
    {

        #region inspector
        
        public UniGraph sourceGraphPrefab;

        #endregion

        private UniGraph targetGraph;

        protected override void OnNodeInitialize()
        {
            
            base.OnNodeInitialize();
            
            if (!sourceGraphPrefab) {
                return;
            }
            
            //create node port values by target graph
            sourceGraphPrefab.Inputs.ForEach(x => this.UpdatePortValue(x.fieldName, x.direction));
            sourceGraphPrefab.Outputs.ForEach(x => this.UpdatePortValue(x.fieldName, x.direction));

        }

        protected override void OnExecute()
        {
            base.OnExecute();
            
            if (!sourceGraphPrefab) {
                return;
            }

            if (!targetGraph) {
                targetGraph = sourceGraphPrefab.Spawn(transform);
            }

            targetGraph.Execute();

            foreach (var port in PortValues) {
                var portName = port.ItemName;
                var originPort = GetPort(portName);
                var targetPort = targetGraph.GetPortValue(portName);
                ConnectToGraphPort(port,targetPort, originPort.direction);
            }
            
            LifeTime.AddCleanUpAction(() => targetGraph?.Exit());
        }

        private void ConnectToGraphPort(IPortValue sourcePort, IPortValue targetPort, PortIO direction)
        {
            var source    = direction == PortIO.Input ? sourcePort : targetPort;
            var target    = direction == PortIO.Input ? targetPort : sourcePort;

            source.Connect(target);
        }

        
    }
}
