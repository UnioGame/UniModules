namespace UniGreenModules.UniNodeSystem.Runtime
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;

    public class NodeContextData : IPoolable
    {
        public UniNode Node;

        public IContext Context;

        public void Activate(UniNode node, IContext nodeContext)
        {
            Release();
            Node = node;
            Context = nodeContext;

            node.Input.Add(nodeContext);
            Node.Execute(Context);

        }
		
        public void Release()
        {
            Node?.Input.Remove<IContext>();
            Node?.Exit();
            Node = null;
            Context = null;
        }
    }
}