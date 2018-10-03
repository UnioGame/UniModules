using UniRx.Async;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IUniTaskExecutor
    {

        UniTask Execute(UniTask actionTask);

        void Stop();

    }
}
