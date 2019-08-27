using UniGreenModules.UniCore.Runtime.DataFlow;
using UniGreenModules.UniCore.Runtime.Interfaces;

namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using System;
    using Runtime.Extensions;
    using Runtime.Interfaces;

    [Serializable]
    public class ConnectedPortPairCommands : ILifeTimeCommand
    {
        protected IPortValue inputPort;
        protected IPortValue outputPort;

        public void Initialize(IUniNode node,
            string input, 
            string output, bool connect = true)
        {
            var ports = node.CreatePortPair(input, output,connect);
            inputPort = ports.inputValue;
            outputPort = ports.outputValue;
        }
        
        public virtual void Execute(ILifeTime lifeTime){}

    }
}
