namespace UniGreenModules.UniCore.Runtime.Rx
{
    using System;
    using Interfaces;
    using Interfaces.Rx;
    using ObjectPool;

    public class RecycleActionObserver<T> : IRecycleObserver<T>
    {
        private Action<T> _onNext;
        private Action _onComplete;
        private Action<Exception> _onError;
    
        public void Initialize(Action<T> onNext, Action onComplete = null, Action<Exception> onError = null)
        {
            _onNext = onNext;
            _onComplete = onComplete;
            _onError = onError;
        }
    
        public void OnCompleted()
        {
            _onComplete?.Invoke();
        }

        public void OnError(Exception error)
        {
            _onError?.Invoke(error);
        }

        public void OnNext(T value)
        {
            _onNext?.Invoke(value);
        }

        public void Release()
        {
            
            OnCompleted();

            _onNext = null;
            _onError = null;
            _onComplete = null;

        }

        public void MakeDespawn()
        {
            this.Despawn();
        }
    }
}
