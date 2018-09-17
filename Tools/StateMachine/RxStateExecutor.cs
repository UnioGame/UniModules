using System;
using System.Collections;
using Assets.Scripts.Extensions;
using UniRx;

namespace Assets.Scripts.Tools.StateMachine
{
    public class RxStateExecutor : IStateExecutor<IEnumerator>
    {
        private IDisposable _exucutionDisposable;

        public void Dispose()
        {
            _exucutionDisposable.Cancel();
        }

        public void Execute(IEnumerator state)
        {
            var enumerator = state;
            _exucutionDisposable = Observable.FromCoroutine(x => enumerator).Subscribe();
        }

        public void Stop()
        {
            _exucutionDisposable.Cancel();
            _exucutionDisposable = null;
        }
    }
}
