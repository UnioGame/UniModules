using System;
using System.Collections;
using Assets.Tools.UnityTools.Extension;
using Assets.Tools.UnityTools.StateMachine.Interfaces;
using UniRx;

namespace Assets.Tools.UnityTools.StateMachine
{
    public class RxStateExecutor : IStateExecutor<IStateBehaviour<IEnumerator>>
    {
        private IDisposable _disposables;
        private IStateBehaviour<IEnumerator> _state;

        public void Execute(IStateBehaviour<IEnumerator> state)
        {
            _state = state;
            _disposables = Observable.FromCoroutine(x => _state.Execute()).Subscribe();
        }

        public void Stop()
        {
            if(_state!=null)
                _state.Exit();
            _state = null;
            _disposables.Cancel();
            _disposables = null;
        }
    }
}
