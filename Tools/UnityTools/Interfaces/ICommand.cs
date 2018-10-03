using System.Collections;

namespace Assets.Tools.UnityTools.Interfaces
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

    public interface IRoutine<out TResult>
    {
        TResult Execute();
    }

    public interface IRoutine<in TData,out TResult>
    {
        TResult Execute(TData data);
    }
}