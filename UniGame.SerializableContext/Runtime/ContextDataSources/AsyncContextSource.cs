namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using System;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using AddressableTools.Runtime.Extensions;
    using UniCore.Runtime.Interfaces;
    using UniModules.UniGame.Context.Runtime.Abstract;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AddressableContextSource", fileName = nameof(AsyncContextSource))]
    public class AsyncContextSource : AsyncContextDataSource
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

    }
}
