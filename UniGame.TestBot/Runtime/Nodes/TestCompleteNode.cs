using Cysharp.Threading.Tasks;
using UniGame.UniNodes.Nodes.Runtime.Common;
using UniModules.UniGame.Core.Runtime.Interfaces;

namespace UniModules.UniGame.TestBot.Runtime.Nodes
{
    using System;
    using UniModules.GameFlow.Runtime.Attributes;
    using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;

    
    [CreateNodeMenu("UniGame/TestsFlow/" + nameof(TestCompleteNode))]
    [NodeInfo(name:nameof(TestCompleteNode),category:"Test",description:"Complete Test Bot Execution")]
    [Serializable]
    public class TestCompleteNode : SContextNode
    {
        protected override UniTask OnContextActivate(IContext context)
        {
            return UniTask.CompletedTask;
        }
    }
}
