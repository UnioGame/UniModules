using System.Collections;
using UniRx.Async;

namespace Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface ICommand
    {
        void Execute();
    }

    public interface IRollbackCommand : ICommand
    {
        bool Rollback();
    }

    public interface ICommandRoutine : IRoutine<IEnumerator> {}

    public interface IRoutine<TResult>
    {
        TResult Execute();
    }

    public interface IRoutine<TData,TResult>
    {
        TResult Execute(TData data);
    }
}