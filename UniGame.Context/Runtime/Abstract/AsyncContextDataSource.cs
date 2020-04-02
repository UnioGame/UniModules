namespace UniModules.UniGame.Context.Runtime.Abstract
{
    using Core.Runtime.ScriptableObjects;
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public abstract class AsyncContextDataSource : 
        LifetimeScriptableObject, 
        IAsyncContextDataSource

    {

        public abstract UniTask<IContext> RegisterAsync(IContext context);

        
    }
}
