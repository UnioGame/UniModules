namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using Addressables;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameSystem/Sources/AsyncContext", fileName = nameof(AsyncContextSource))]
    public class AsyncContextSource : AsyncContextDataSource
    {
        public ContextSourceAssetReference contextAsset;
    
        public override async UniTask<IContext> RegisterAsync(IContext context)
        {
            var contextReference = await contextAsset.LoadAssetAsync().Task;
            await contextReference.RegisterAsync(context);
            return context;
        }    
    }
}
