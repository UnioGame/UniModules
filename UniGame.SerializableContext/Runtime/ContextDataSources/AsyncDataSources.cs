namespace UniModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Context.Runtime.Abstract;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Addressables;
    using UniRx.Async;
    using UnityEngine;

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
            target.LifeTime.AddCleanUpAction(() => GameLog.Log($"{name} {target.GetType().Name} END LIFETIME CONTEXT"));
            
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
