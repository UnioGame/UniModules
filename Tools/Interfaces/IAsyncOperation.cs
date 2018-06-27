using Assets.Tools.Utils;

namespace Tools.AsyncOperations
{
    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
        bool Active { get; }
    }
}
