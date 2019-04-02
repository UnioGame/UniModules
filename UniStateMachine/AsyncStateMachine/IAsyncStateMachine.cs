using System;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}