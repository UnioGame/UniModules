namespace UniModules.UniGame.Context.Runtime.Abstract
{
    using Core.Runtime.ScriptableObjects;
    using Cysharp.Threading.Tasks;
    using UniGreenModules.UniContextData.Runtime.Interfaces;
    using UniGreenModules.UniCore.Runtime.Interfaces;

    public abstract class AsyncContextDataSource : 
        LifetimeScriptableObject, 
        IAsyncContextDataSource

    {

        public abstract UniTask<IContext> RegisterAsync(IContext context);

        
    }
}
