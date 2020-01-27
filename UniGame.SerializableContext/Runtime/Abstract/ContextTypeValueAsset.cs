namespace UniGreenModules.UniGame.SerializableContext.Runtime.Abstract
{
    using UniContextData.Runtime.Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public class ContextTypeValueAsset<TValue,TApiValue> : 
        TypeValueDefaultAsset<TValue,TApiValue> ,
        IAsyncContextDataSource
        where TValue : TApiValue, new()
    {
        public virtual async UniTask<IContext> RegisterAsync(IContext context)
        {
            context.Publish(Value);
            return context;
        }
    }
}
