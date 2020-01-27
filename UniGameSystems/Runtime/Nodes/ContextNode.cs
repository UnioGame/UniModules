namespace UniGreenModules.UniGameSystems.Runtime.Nodes
{
    using System.Collections.Generic;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.AddressableTools.Runtime.Extensions;
    using UniGame.SerializableContext.Runtime.Addressables;
    using UniGame.SerializableContext.Runtime.AssetTypes;
    using UniGameFlow.UniNodesSystem.Assets.UniGame.UniNodes.Nodes.Runtime.Nodes;
    using UniRx.Async;
    using UnityEngine;

    [CreateNodeMenu("GameSystem/DataContext")]
    public class ContextNode : InOutPortNode
    {
        [Header("Context")]
        public ContextSourceAssetReference contextAsset;

        [Header("Node Context Data Source")]
        public List<ScriptableObjectAssetReference> contextDataSources = new List<ScriptableObjectAssetReference>();

        protected override async void OnExecute()
        {
            var output = PortPair.OutputPort;
            IContext context = null;

            if (contextAsset.RuntimeKey == null && contextDataSources.Count <= 0) {
                GameLog.LogError($"{graph.name}:{name} EMPTY Asset References");
                return;
            }

            var contextReference = await contextAsset.LoadAssetTaskAsync<ContextAsset>();
            if (contextReference != null) {
                context = contextReference.Value;
            }
                
            LogStatus(contextReference);

            for (var i = 0; i < contextDataSources.Count; i++) {
                var contextSource = contextDataSources[i];
                await RegisterContexts(context, contextSource);
            }

            await contextReference.RegisterAsync(output);
                
        }

        private async UniTask<bool> RegisterContexts(IContext target,ScriptableObjectAssetReference sourceReference)
        {
            var source = await sourceReference.LoadAssetTaskAsync<ScriptableObject>();
            if (!(source is IAsyncContextDataSource dataSource)) {
                return false;
            }

            await dataSource.RegisterAsync(target);
            return true;

        }
        
        private void LogStatus<T>(T asset)
        {
            GameLog.Log(asset == null ? 
                $"NULL ASSET {typeof(T).Name} FROM assetReference {name}" : 
                $"DONE {asset}");
        }
        
    }
}
