using UniModule.UnityTools.Interfaces;

namespace UniStateMachine.Nodes
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.ObjectPool;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public class UniGraphContextData : IPoolable
    {
        public IContext GraphContext;

        public NodeContextData NodeContextData;

        public void ActivateNode(UniNode node, IContext context, IContext graphContext)
        {
            Release();
            GraphContext = graphContext;
            NodeContextData = ClassPool.Spawn<NodeContextData>();
            NodeContextData.Activate(node,context);
        }

        public void Release()
        {
            GraphContext = null;
            NodeContextData?.Release();
        }
    }
}