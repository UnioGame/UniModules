using Assets.Tools.Utils;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
