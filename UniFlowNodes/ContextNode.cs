namespace UniGreenModules.UniFlowNodes
{
    using System.Collections.Generic;
    using UniCore.Runtime.Interfaces;
    using UniNodeSystem.Nodes.Commands;
    using UniNodeSystem.Runtime;

    public class ContextNode : UniNode
    {

        public const string ContextPortName = "context";

        protected override void OnNodeInitialize()
        {
            
        }

        protected override void UpdateNodeCommands(List<ILifeTimeCommand> nodeCommands)
        {
            base.UpdateNodeCommands(nodeCommands);
            var portCommand = new ConnectedFormatedPairCommand();
        }
    }
}
