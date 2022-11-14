﻿namespace UniModules.UniStateMachine.Runtime.AsyncStateMachine
{
    using System;
    using Cysharp.Threading.Tasks;
    using Runtime.Interfaces;
    using UniCore.Runtime.Extension;
    using UniRx;
    

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
