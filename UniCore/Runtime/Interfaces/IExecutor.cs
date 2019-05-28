namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System.Collections;

    public interface IExecutor : IRoutineExecutor<IEnumerator>{}

    public interface IRoutineExecutor<TAwaiter>: 
        ICommonExecutor<TAwaiter>
    {
        bool IsActive { get; }
    }

    public interface ICommonExecutor<TData>
    {
        void Execute(TData data);
        void Stop();
    }

    public interface IContextExecutor<TContext>
    {
        IDisposableItem Execute(TContext context);
    }

    public interface IExecutor<out TAwaiter>
    {
        TAwaiter Execute();
    }
}