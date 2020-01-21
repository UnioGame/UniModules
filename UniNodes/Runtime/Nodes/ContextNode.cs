namespace UniGreenModules.UniNodes.Runtime.Nodes
{
    using System;
    using UniCore.Runtime.Interfaces;
    using UniFlowNodes;

    [Serializable]
    [CreateNodeMenu("Common/ContextNode")]
    public class ContextNode : TypeBridgeNode<IContext>
    {
        
    }
}
