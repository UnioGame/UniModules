using System.Collections;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IExecutor : ICommonExecutor<IEnumerator> { }

    public interface IProcess
    {
        bool IsActive { get; }
        
        void Execute();
        void Stop();
    }
    
    public interface ICommonExecutor<TData>
    {
        void Execute(TData data);
        void Stop();
    }
}