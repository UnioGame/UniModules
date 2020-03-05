namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System;
    using Abstract;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using AddressableTools.Runtime.Extensions;
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
            var contextReference = await contextAsset.LoadAssetTaskAsync(LifeTime);
            await contextReference.RegisterAsync(context);

            if (contextAsset.Asset is IDisposable disposable)
                LifeTime.AddDispose(disposable);
            
            LifeTime.AddCleanUpAction(contextAsset.ReleaseAsset);
            
            return context;
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
