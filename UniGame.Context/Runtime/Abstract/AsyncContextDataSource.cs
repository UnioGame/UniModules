namespace UniGreenModules.UniGame.Context.Runtime.Interfaces
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public abstract class AsyncContextDataSource : 
        DisposableScriptableObject, 
        IAsyncContextDataSource

    {

        public abstract UniTask<IContext> RegisterAsync(IContext context);

        
    }
}
