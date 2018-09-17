using System;

namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}