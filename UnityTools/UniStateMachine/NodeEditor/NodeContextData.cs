using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using Assets.Tools.UnityTools.UniRoutine;

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

            node.Input.Value.AddValue(nodeContext,nodeContext);
            Node.Execute(Context);

        }
		
        public void Release()
        {
            Node?.Input.Value.RemoveContext(Context);
            Node?.Exit(Context);
            Node = null;
            Context = null;
        }
    }
}