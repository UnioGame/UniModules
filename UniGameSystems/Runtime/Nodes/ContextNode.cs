namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniGame.SerializableContext.Runtime.Scriptable;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UnityEngine;

    [CreateNodeMenu("GameSystem/DataContext")]
    public class ContextNode : InOutPortNode
    {
        [Header("Context")]
        public ContextSourceAssetReference contextAsset;
        
        [Header("Node Context Data Source")]
        public ContextDataSourceAssetReference contextDataSource;

        protected override async void OnExecute()
        {
            var output = PortPair.OutputPort;
            IContext context = null;

            if (contextAsset.RuntimeKey == null && contextDataSource.RuntimeKey == null) {
                GameLog.LogError($"{graph.name}:{name} EMPTY Asset References");
                return;
            }

            if (contextAsset.RuntimeKey != null) {
                var contextHandler = contextAsset.LoadAssetAsync();
                var contextReference = await contextHandler.Task;
                await contextReference.RegisterAsync(output);
                context = contextReference.Value;
                
                LogStatus(contextReference);
            }

            if (contextDataSource.RuntimeKey == null) {
                return;
            }

            var assetHandler = 
                contextDataSource.LoadAssetAsync<AsyncContextDataSource>();
            var asset = await assetHandler.Task;

            if(context!=null) 
                await asset.RegisterAsync(context);
                
            await asset.RegisterAsync(output);
                
            LogStatus(asset);
        }

        private void LogStatus<T>(T asset)
        {
            GameLog.Log(asset == null ? 
                $"NULL ASSET {typeof(T).Name} FROM assetReference {name}" : 
                $"DONE {asset}");
        }
        
    }
}
