namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using UniRx.Async;

    public interface IUniTaskExecutor
    {

        UniTask Execute(UniTask actionTask);

        void Stop();

    }
}
