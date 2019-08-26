namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    public interface ICommand
    {
        void Execute();
    }

    public interface IRoutine<out TResult>
    {
        TResult Execute();
    }

    public interface IRoutine<in TData,out TResult>
    {
        TResult Execute(TData data);
    }
}