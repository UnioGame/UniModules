using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniStateMachine.Nodes
{
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