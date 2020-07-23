namespace UniGreenModules.UniContextData.Runtime.Interfaces
{
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.Interfaces;

    public interface IAsyncContextDataSource
    {
        UniTask<IContext> RegisterAsync(IContext context);
    }
}
