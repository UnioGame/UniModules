namespace UniGreenModules.UniNodeSystem.Runtime.BaseNodes
{
    using Interfaces;
    using Runtime;

    public class GraphOuputNode : UniNode, IGraphPortNode
    {
        
        public PortIO Direction => PortIO.Output;

        public UniPortValue PortValue => Output;

    }
    
}
