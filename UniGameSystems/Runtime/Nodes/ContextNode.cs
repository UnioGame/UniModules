namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.AddressableTools.Runtime.Extensions;
    using UniGame.SerializableContext.Runtime;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniGame.SerializableContext.Runtime.AssetTypes;
    using UniGame.SerializableContext.Runtime.Scriptable;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

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

            var contextReference = await contextAsset.LoadAssetTaskAsync<ContextAsset>();
            if (contextReference != null) {
                await contextReference.RegisterAsync(output);
                context = contextReference.Value;
            }
                
            LogStatus(contextReference);
            
            var asset = await contextDataSource.LoadAssetTaskAsync<AsyncContextDataSource>();
            LogStatus(asset);
    
            if (asset == null) return;
            
            if(context!=null) 
                await asset.RegisterAsync(context);
                
            await asset.RegisterAsync(output);
                
        }

        private void LogStatus<T>(T asset)
        {
            GameLog.Log(asset == null ? 
                $"NULL ASSET {typeof(T).Name} FROM assetReference {name}" : 
                $"DONE {asset}");
        }
        
    }
}
