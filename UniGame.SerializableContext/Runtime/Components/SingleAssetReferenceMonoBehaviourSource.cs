namespace Taktika.GameResources
{
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx.Async;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class SingleAssetReferenceMonoBehaviourSource<TObject, TApi> : AsyncContextDataSource
        where TObject : MonoBehaviour, TApi
    {
        [SerializeField] 
        private AssetReferenceGameObject _prefabReference;

        private static TObject _instance;

        public TObject Asset => _instance;

        public sealed override async UniTask<IContext> RegisterAsync(IContext context)
        {
            GameLog.Log($"{GetType().Name} TRY REGISTER {typeof(TApi).Name}",Color.blue);

            context.LifeTime.AddCleanUpAction(() => GameLog.Log($"Context {context.GetType().Name} {GetType().Name} LIFETIME FINISHED",Color.red));
            
            var asset = await _prefabReference.LoadGameObjectAssetTaskAsync<TObject>(context.LifeTime);
            lock (this) {
                if (!_instance) {
                    if (!asset) {
                        GameLog.LogError($"{GetType().Name} NULL resource load from {_prefabReference}");
                        return context;
                    }
                    _instance = Instantiate(asset).GetComponent<TObject>();
                }
            }
            if (!_instance) {
                GameLog.LogError($"{GetType().Name} NULL TObject {nameof(TObject)} load from {_prefabReference}");
                return context;
            }

            var targetAsset = await OnInstanceReceive(_instance,context);
            
            context.Publish<TApi>(targetAsset);

            OnInstanceRegistered(_instance, context);

            return context;
        }

        protected virtual async UniTask<TApi> OnInstanceReceive(TObject asset,IContext context)
        {
            return asset;
        }
        
        protected virtual void OnInstanceRegistered(TObject asset, IContext context)
        {
            
        }

    }
}