using System.Collections;
using UniRx.Async;

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
