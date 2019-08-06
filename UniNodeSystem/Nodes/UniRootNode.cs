namespace UniGreenModules.UniNodeSystem.Nodes
{
    using Runtime;
    using Runtime.Interfaces;
    using Runtime.Runtime;

    public class UniRootNode : UniNode, IUniRootNode{
        
        public PortIO Direction => PortIO.Input;

        public IPortValue PortValue { get; protected set; }

        public bool Visible => false;
    }
        
}
