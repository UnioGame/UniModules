using UniRx.Async;

namespace UniModule.UnityTools.Interfaces
{
    public interface IUniTaskExecutor
    {

        UniTask Execute(UniTask actionTask);

        void Stop();

    }
}
