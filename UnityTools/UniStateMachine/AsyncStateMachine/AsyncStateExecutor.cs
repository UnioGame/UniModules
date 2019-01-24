using System;
using UniModule.UnityTools.Extension;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx;
using UniRx.Async;

namespace UniModule.UnityTools.UniStateMachine.AsyncStateMachine
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
                FromCoroutine(() => task.ToCoroutine(ExeptionHandler)).
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
