namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System;
    using Abstract;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using Context.Runtime.Interfaces;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressableContextSource", fileName = nameof(AsyncContextSource))]
    public class AsyncContextSource : AsyncContextDataSource , IResourceDisposable
    {
        [ShowAssetReference]
        public ContextAssetReference contextAsset;


        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            var contextReference = await contextAsset.LoadAssetAsync().Task;
            await contextReference.RegisterAsync(context);
            return context;
        }

        public override void Dispose()
        {
            if(contextAsset.Asset is IDisposable disposable)
                disposable?.Dispose();
            
            contextAsset.ReleaseAsset();
        }

        protected override void OnSourceEnable(ILifeTime lifeTime)
        {
            base.OnSourceEnable(lifeTime);
            if (disposeOnUnload) {
                lifeTime.AddDispose(this);
            }
        }
    }
}
