namespace UniGreenModules.UniStateMachine.Runtime.AsyncStateMachine
{
    using System;

    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}