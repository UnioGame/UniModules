namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using ObjectPool.Interfaces;

    public interface IAsyncOperation : IPoolable, ICommandRoutine
    {
        bool IsDone { get; }
        string Error { get; }
    }
}
