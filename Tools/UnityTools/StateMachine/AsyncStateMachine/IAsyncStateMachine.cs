using System;

namespace UniStateMachine
{
    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}