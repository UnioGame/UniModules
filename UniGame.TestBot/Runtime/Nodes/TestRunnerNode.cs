using System;
using Cysharp.Threading.Tasks;
using UniGame.UniNodes.Nodes.Runtime.Common;
using UniModules.GameFlow.Runtime.Attributes;
using UniModules.UniGame.AddressableTools.Runtime.Extensions;
using UniModules.UniGame.Core.Runtime.Interfaces;
using UniModules.UniGame.TestBot.Runtime;
using UniModules.UniGameFlow.NodeSystem.Runtime.Core.Attributes;
using UnityEngine.AddressableAssets;

[CreateNodeMenu("UniGame/TestsFlow/" + nameof(TestRunnerNode))]
[NodeInfo(name:nameof(TestRunnerNode),category:"Test",description:"Run test flow at runtime graph")]
[Serializable]
public class TestRunnerNode : SContextNode
{

    public AssetReferenceT<TestScenarioRunner> testRunner;

    protected sealed override async UniTask OnContextActivate(IContext context)
    {
        var runner = await testRunner.LoadAssetTaskAsync(LifeTime);

        var result = await runner.Execute();

    }
}
