namespace UniGreenModules.UniNodeSystem.Runtime
{
    using Interfaces;
    using UniCore.Runtime.Interfaces;

    public interface INodeCommand : ICommand
    {
        /// <summary>
        /// attach to target node, create all ports or data
        /// </summary>
        /// <param name="targetNode"></param>
        void AttachToNode(IUniNode targetNode);

    }
}