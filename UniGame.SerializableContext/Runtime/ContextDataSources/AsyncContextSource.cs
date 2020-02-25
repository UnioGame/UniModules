namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using Abstract;
    using Addressables;
    using AddressableTools.Runtime.Attributes;
    using Context.Runtime.Interfaces;
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

        public void Dispose()
        {
            contextAsset.ReleaseAsset();
        }
    }
}
