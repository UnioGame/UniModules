using System;
using UniRx.Async;

namespace Assets.Tools.UnityTools.StateMachine.AsyncStateMachine
{
    public interface IAsyncStateExecutor : IDisposable
    {
        void Execute(UniTask task);
        void Stop();
    }

}



