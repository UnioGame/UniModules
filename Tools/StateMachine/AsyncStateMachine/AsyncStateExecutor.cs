using System;
using System.Collections;
using System.Threading;
using Assets.Scripts.Extensions;
using UniRx;
using UniRx.Async;

namespace Assets.Scripts.Tools.StateMachine.AsyncStateMachine
{
  
    public class AsyncStateExecutor : IStateExecutor<UniTask>
    {
        private IDisposable _taskDisposable;

        public void Dispose()
        {
            Stop();
        }

        public void Execute(UniTask state)
        {
            Stop();
            var task = state;
            _taskDisposable = Observable.
                FromMicroCoroutine(() => task.ToCoroutine(ExeptionHandler)).
                Subscribe();
        }

        public void Stop()
        {
            _taskDisposable.Cancel();
        }

        private void ExeptionHandler(Exception exception)
        {
            throw exception;
        }

    }
    
}
