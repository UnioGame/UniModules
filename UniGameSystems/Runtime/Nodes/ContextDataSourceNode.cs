using UniGreenModules.UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;

namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using UniCore.Runtime.ProfilerTools;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UnityEngine;

    [CreateNodeMenu("GameSystem/ContextDataSource")]
    public class ContextDataSourceNode : InOutPortNode
    {
        
        [Header("Node Context Data Source")]
        public ContextDataSourceAssetReference contextDataSource;

        protected override async void OnExecute()
        {
            if (contextDataSource.RuntimeKey == null) {
                GameLog.LogError($"{graph.name}:{name} {nameof(contextDataSource)} is EMPTY");
                return;
            }

            var handler = contextDataSource.LoadAssetAsync();
            var asset = await handler.Task;
            
            var output = PortPair.OutputPort;
            await asset.RegisterAsync(output);

        }
    }
}
