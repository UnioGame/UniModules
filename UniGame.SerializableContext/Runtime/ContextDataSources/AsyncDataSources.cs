using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using AddressableTools.Runtime.Extensions;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx.Async;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressablesAsyncSources", fileName = nameof(AsyncDataSources))]
    public class AsyncDataSources : AsyncContextDataSource
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        [ShowAssetReference]
        public List<AssetReferenceScriptableObject> sourceAssets = new List<AssetReferenceScriptableObject>();
        

        #endregion
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            
            for (var i = 0; i < sourceAssets.Count; i++) {
                var contextSource = sourceAssets[i];
                await RegisterContexts(context, contextSource);
            }
            
            return context;
        }
        
        
        private async UniTask<bool> RegisterContexts(IContext target,AssetReferenceScriptableObject sourceReference)
        {
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
