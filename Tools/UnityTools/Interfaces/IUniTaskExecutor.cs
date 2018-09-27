using UniRx.Async;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IUniTaskExecutor
    {

        UniTask Execute(UniTask actionTask);

        void Stop();

    }
}
