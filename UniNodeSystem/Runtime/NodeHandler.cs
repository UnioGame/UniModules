namespace UniGreenModules.UniNodeSystem.Runtime
{
    using Interfaces;

    public class NodeHandler
    {
        private IUniNode node;
        
        /// <summary>
        /// attach to target node, create all ports or data
        /// </summary>
        /// <param name="targetNode"></param>
        public void AttachToNode(IUniNode targetNode)
        {
            node = targetNode;
        }

        public void Execute()
        {
            
        }

        protected virtual void OnInitialize()
        {
            
        }
        
    }
}
