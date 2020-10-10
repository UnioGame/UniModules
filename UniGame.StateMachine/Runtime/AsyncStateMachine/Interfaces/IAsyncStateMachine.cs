namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine.Interfaces
{
    using System;

    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}