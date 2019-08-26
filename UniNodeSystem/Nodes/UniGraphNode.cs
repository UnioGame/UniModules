using UniGreenModules.UniNodeSystem.Runtime;

namespace UniGreenModules.UniNodeSystem.Nodes
{
    using System.Collections.Generic;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;

    public class UniGraphNode : UniNode
    {

        #region inspector
        
        public UniGraph targetGraph;

        #endregion


        protected override void OnNodeInitialize()
        {
            
            if (!targetGraph) {
                ClearInstancePorts();
                return;
            }
            
            base.OnNodeInitialize();


            ConnectToGraphPorts(targetGraph.InputsPorts);
            ConnectToGraphPorts(targetGraph.OutputsPorts);

        }

        protected override void OnExecute()
        {
            base.OnExecute();
            
            if (!targetGraph) {
                return;
            }

            targetGraph.Execute();
            LifeTime.AddCleanUpAction(() => targetGraph.Exit());
        }

        private void ConnectToGraphPorts(IReadOnlyList<IGraphPortNode> ports)
        {
            foreach (var portNode in ports) {
                var direction = portNode.Direction;
                var pair =this.UpdatePortValue(portNode.ItemName, 
                                               portNode.Direction);

                var portValue = pair.value;
                var source = direction == PortIO.Input ? portValue : portNode.PortValue;
                var target = direction == PortIO.Input ? portNode.PortValue : portValue;

                source.Connect(target);
            }
        }

        
    }
}
