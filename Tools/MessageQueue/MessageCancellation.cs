using System;
using System.Collections.Generic;
using Assets.Tools.Utils;
using UniRx;

namespace Assets.Scripts.MessageQueue
{
    public class MessageCancellation<T> : IPoolable, IResetable
    {
        private IObserver<T> _observer;
        private List<IObserver<T>> _disposables;

        public void Initialize(IObserver<T> observer, List<IObserver<T>> disposables) {
            _observer = observer;
            _disposables = disposables;
        }

        public void Release() {
            _observer.OnCompleted();
            _disposables.Add(_observer);

            _observer = null;
            _disposables = null;
        }

        public void Reset() {
            Release();
        }
    }
}
