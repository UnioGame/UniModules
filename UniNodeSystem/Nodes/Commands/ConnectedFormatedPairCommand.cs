namespace UniGreenModules.UniNodeSystem.Nodes.Commands
{
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Runtime;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Interfaces;

    public class ConnectedFormatedPairCommand : ILifeTimeCommand
    {
        public IPortValue InputPort { get; protected set; }
        
        public IPortValue OutputPort { get; protected set; }
        
        public void Initialize(
            IUniNode node, 
            string input, 
            bool connect = true)
        {
            var inputName = input.GetFormatedPortName(PortIO.Input);
            var outputName = input.GetFormatedPortName(PortIO.Output);
            
            var ports = node.CreatePortPair(inputName, outputName, connect);
            
            InputPort = ports.inputValue;
            OutputPort = ports.outputValue;
        }
        
        public virtual void Execute(ILifeTime lifeTime) {}
        
    }
}
