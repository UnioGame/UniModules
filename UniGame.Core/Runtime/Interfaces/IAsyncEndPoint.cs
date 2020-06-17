namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx.Async;

    public interface IAsyncEndPoint<T>
    {
        UniTask<T> Exit();
    }
}