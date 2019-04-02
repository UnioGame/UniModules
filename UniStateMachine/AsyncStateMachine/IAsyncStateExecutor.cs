using System;
using UniRx.Async;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
{
    public interface IAsyncStateExecutor : IDisposable
    {
        void Execute(UniTask task);
        void Stop();
    }

}



