namespace UniGreenModules.UniContextData.Runtime.Interfaces
{
    using UniCore.Runtime.Interfaces;
    using UniRx.Async;

    public interface IAsyncContextDataSource
    {
        UniTask<IContext> RegisterAsync(IContext context);
    }
}
