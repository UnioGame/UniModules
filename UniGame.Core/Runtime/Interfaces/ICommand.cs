namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface ICommand
    {
        void Execute();
    }

    public interface IDisposableCommand : ICommand, IDisposable{}
    
    public interface IRoutine<out TResult>
    {
        TResult Execute();
    }

    public interface IRoutine<in TData,out TResult>
    {
        TResult Execute(TData data);
    }
}