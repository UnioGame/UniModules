using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Addressables;
    using AddressableTools.Runtime.Extensions;
    using Context.Runtime.Interfaces;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    
    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AsyncDataSources", fileName = nameof(AsyncDataSources))]
    public class AsyncDataSources : AsyncContextDataSource
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        public List<ScriptableObjectAssetReference> sourceAssets = new List<ScriptableObjectAssetReference>();
        
        #endregion
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            
            for (var i = 0; i < sourceAssets.Count; i++) {
                var contextSource = sourceAssets[i];
                await RegisterContexts(context, contextSource);
            }
            
            return context;
        }
        
        
        private async UniTask<bool> RegisterContexts(IContext target,ScriptableObjectAssetReference sourceReference)
        {
            var source = await sourceReference.LoadAssetTaskAsync<IAsyncContextDataSource>();
            if (source == null) {
                return false;
            }
            await source.RegisterAsync(target);
            return true;

        }

    }
}
