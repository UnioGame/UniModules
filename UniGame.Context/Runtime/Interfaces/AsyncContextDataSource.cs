namespace UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;
    using UnityEngine;

    public abstract class AsyncContextDataSource : 
        ScriptableObject, 
        IAsyncContextDataSource
    {
        
        public abstract UniTask<IContext> RegisterAsync(IContext context);

    }
}
