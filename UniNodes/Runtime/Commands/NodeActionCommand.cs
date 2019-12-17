namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System;
    using Runtime.Core;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;

    [Serializable]
    public class NodeActionCommand<T> : ILifeTimeCommand
    {
        private PortActionCommand<T> portAction;

        public NodeActionCommand(
            Action<T> action, 
            IUniNode node, 
            string name, 
            PortIO direction = PortIO.Input)
        {
            var portInfo = node.UpdatePortValue(name, direction);
            portAction = new PortActionCommand<T>(action,portInfo.value);
        }
        
        public void Execute(ILifeTime lifeTime)
        {
            portAction.Execute(lifeTime);
        }
    }
}
