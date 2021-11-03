using Cysharp.Threading.Tasks;
using UniGame.UniNodes.Nodes.Runtime.Common;
using UniModules.GameFlow.Runtime.Attributes;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;

[CreateNodeMenu("UniGame/TestsFlow/" + nameof(TestRunnerNode))]
[NodeInfo(name:nameof(TestRunnerNode),category:"Test",description:"Run test flow at runtime graph")]
public class TestRunnerNode : SContextNode
{
    protected sealed override UniTask OnContextActivate(IContext context)
    {
        return UniTask.CompletedTask;
    }
}
