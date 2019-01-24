using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.Interfaces
{
    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
