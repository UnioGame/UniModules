namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface IProcess
    {
        bool IsActive { get; }
        void Execute();
        void Stop();
    }
}