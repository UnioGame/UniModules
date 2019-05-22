using System;
using System.Collections;
using UniModule.UnityTools.UniStateMachine.Interfaces;
using UniRx;

namespace UniModule.UnityTools.UniStateMachine
{
    using UniGreenModules.UniCore.Runtime.Extension;

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
