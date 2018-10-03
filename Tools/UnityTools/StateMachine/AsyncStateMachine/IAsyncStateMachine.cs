using System;

namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateMachine : IDisposable
    {
        void Execute(IAsyncStateBehaviour state);
        void Stop();
    }
}