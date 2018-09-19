using System;
using System.Collections;
using Assets.Scripts.Extensions;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public class RxStateExecutor : IStateExecutor<IEnumerator>
    {
        private IDisposable _disposables;

        public void Dispose()
        {
            _disposables.Cancel();
        }

        public void Execute(IEnumerator state)
        {
            var enumerator = state;
            _disposables = Observable.FromCoroutine(x => enumerator).Subscribe();
        }

        public void Stop()
        {
            _disposables.Cancel();
            _disposables = null;
        }
    }
}
