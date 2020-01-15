using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes
{
    using Runtime.Core;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;

    public abstract class UniGraphNode : UniNode
    {
        
        public abstract UniGraph LoadOrigin();
        
        protected override void OnInitialize()
        {
            
            base.OnInitialize();

            var sourceGraphPrefab = LoadOrigin();
            
            if (!sourceGraphPrefab) {
                return;
            }
            
            //create node port values by target graph
            foreach (var input in sourceGraphPrefab.Inputs) {
                this.UpdatePortValue(input.fieldName, input.direction);
            }
            foreach (var output in sourceGraphPrefab.Outputs) {
                this.UpdatePortValue(output.fieldName, output.direction);
            }
        }

        protected override void OnExecute()
        {
            base.OnExecute();
            
            var graphPrefab = CreateGraph(LifeTime);
            if (!graphPrefab) {
                return;
            }

            graphPrefab.Execute();

            foreach (var port in PortValues) {
                var portName = port.ItemName;
                var originPort = GetPort(portName);
                var targetPort = graphPrefab.GetPortValue(portName);
                ConnectToGraphPort(port,targetPort, originPort.direction);
            }
            
            LifeTime.AddCleanUpAction(() => graphPrefab?.Exit());
        }

        protected abstract UniGraph CreateGraph(ILifeTime lifeTime);
        
        private void ConnectToGraphPort(IPortValue sourcePort, IPortValue targetPort, PortIO direction)
        {
            var source    = direction == PortIO.Input ? sourcePort : targetPort;
            var target    = direction == PortIO.Input ? targetPort : sourcePort;

            source.Connect(target);
        }
        
        
    }
}
