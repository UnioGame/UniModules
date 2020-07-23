namespace UniModules.UniGame.SerializableContext.Runtime.ContextDataSources
{
    using System.Collections.Generic;
    using Context.Runtime.Abstract;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Addressables;
    using UniRx;
    
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressablesAsyncSources", fileName = nameof(AsyncDataSources))]
    public class AsyncDataSources : AsyncContextDataSource
    {
        #region inspector

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineEditor()]
#endif
        public List<ScriptableObject> sources = new List<ScriptableObject>();

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.LabelText("Async Sources")]
#endif
        public List<AssetReferenceDataSource> sourceAssets = new List<AssetReferenceDataSource>();
        
        #endregion
        
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            foreach (var source in sources) {
                var asyncSource = source as IAsyncContextDataSource;
                if(asyncSource == null) 
                    continue;
                asyncSource.RegisterAsync(context);
            }
            
            await UniTask.WhenAll(sourceAssets.Select(x => RegisterContexts(context, x)));

            return context;
        }
        
        private async UniTask<bool> RegisterContexts(IContext target,AssetReferenceDataSource sourceReference)
        {
            GameLog.Log($"RegisterContexts {name} {target.GetType().Name} LIFETIME CONTEXT");
                
            var lifetime = target.LifeTime;
            lifetime.AddCleanUpAction(() => GameLog.Log($"RegisterContexts {name} {target.GetType().Name} END LIFETIME CONTEXT"));
            
            var source = await sourceReference.LoadAssetTaskAsync(lifetime);
            if (source == null) {
                GameLog.LogError($"Empty Data source found {sourceReference} GUID {sourceReference.AssetGUID}");
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
