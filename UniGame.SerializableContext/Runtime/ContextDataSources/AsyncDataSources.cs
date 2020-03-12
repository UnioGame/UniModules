using UnityEngine;

namespace UniGreenModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Abstract;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using AddressableTools.Runtime.Extensions;
    using Context.Runtime.Interfaces;
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ProfilerTools;
    using UniRx.Async;

    
    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressablesAsyncSources", fileName = nameof(AsyncDataSources))]
    public class AsyncDataSources : AsyncContextDataSource
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.DrawWithUnity]
#endif
        [ShowAssetReference]
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
            var source = await sourceReference.LoadAssetTaskAsync<IAsyncContextDataSource>(target.LifeTime);
            if (source == null) {
                return false;
            }
            await source.RegisterAsync(target);
            return true;

        }

        protected override void OnSourceEnable(ILifeTime lifeTime)
        {
            foreach (var reference in sourceAssets) {
                lifeTime.AddDispose(reference);
            }
        }
        
    }
}
