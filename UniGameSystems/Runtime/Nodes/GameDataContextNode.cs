namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using Addressables;
    using Scriptable;
    using SharedData;
    using UniCore.Runtime.ProfilerTools;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateNodeMenu("GameSystem/DataContext")]
    public class GameDataContextNode : InOutPortNode
    {
        [Header("Node Data Context")]
        public ContextSourceAssetReference dataContext;

        protected override async void OnExecute()
        {
            var assetHandler = 
                dataContext.LoadAssetAsync<AsyncContextDataSource>();
            var asset = await assetHandler.Task;
            
            LogStatus<SharedContext>(asset);

            base.OnExecute();
        }

        private void LogStatus<T>(object asset)
        {
            GameLog.LogError(asset == null ? 
                $"NULL ASSET {typeof(T).Name} FROM assetReference {name}" : 
                $"DONE {asset}");
        }
    }
}
