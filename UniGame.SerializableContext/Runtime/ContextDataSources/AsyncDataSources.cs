using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Addressables;
    using AddressableTools.Runtime.Extensions;
    using global::UniCore.Runtime.ProfilerTools;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx.Async;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressablesAsyncSources", fileName = nameof(AsyncDataSources))]
    public class AsyncDataSources : AsyncContextDataSource
    {
        #region inspector

        public List<AssetReferenceScriptableObject> sourceAssets = new List<AssetReferenceScriptableObject>();
        
        #endregion
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            await UniTask.WhenAll(sourceAssets.Select(x => RegisterContexts(context, x)));

            return context;
        }
        
        private async UniTask<bool> RegisterContexts(IContext target,AssetReferenceScriptableObject sourceReference)
        {
            target.LifeTime.AddCleanUpAction(() => GameLog.Log($"{name} END LIFETIME CONTEXT"));
            
            var source = await sourceReference.LoadAssetTaskAsync<IAsyncContextDataSource>(target.LifeTime);
            if (source == null) {
                return false;
            }
            await source.RegisterAsync(target);
            return true;

        }

        protected override void OnReset()
        {
            foreach (var reference in sourceAssets) {
                LifeTime.AddDispose(reference);
            }
        }
        
    }
}
