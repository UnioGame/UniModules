namespace UniGreenModules.UniNodeSystem.Runtime
{
    using Interfaces;

    public class NodeCommand : INodeCommand
    {
        private IUniNode node;
        
        /// <summary>
        /// attach to target node, create all ports or data
        /// </summary>
        /// <param name="targetNode"></param>
        public void AttachToNode(IUniNode targetNode)
        {
            node = targetNode;
            OnInitialize(node);
        }

        public void Execute(){}

        /// <summary>
        /// initialize
        /// </summary>
        /// <param name="targetNode"></param>
        protected virtual void OnInitialize(IUniNode targetNode){}
        
    }
}
