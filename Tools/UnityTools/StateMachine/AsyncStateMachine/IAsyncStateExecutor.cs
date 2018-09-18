using System;
using UniRx.Async;

namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateExecutor : IDisposable
    {
        void Execute(UniTask task);
        void Stop();
    }

}



