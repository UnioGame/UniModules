using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
