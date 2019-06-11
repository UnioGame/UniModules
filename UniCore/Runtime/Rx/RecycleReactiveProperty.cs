namespace UniGreenModules.UniCore.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Interfaces.Rx;
    using ObjectPool;
    using ObjectPool.Interfaces;

    public class RecycleReactiveProperty<T> : IRecycleReactiveProperty<T>,IDisposable
    {
        private T value = default;
        private bool hasValue;
        
        private Dictionary<IObserver<T>,DisposableAction> _observers = 
            new Dictionary<IObserver<T>, DisposableAction>();

        public T Value {
            get => value;
            set => SetValue(value);
        }

        public bool HasValue => hasValue;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var disposeAction = ClassPool.Spawn<DisposableAction>();
            disposeAction.Initialize(() => Remove(observer));

            _observers[observer] = disposeAction;
            
            observer.OnNext(Value);
            
            return disposeAction;

        }

        public void SetValue(T propertyValue)
        {
            hasValue = true;
            value = propertyValue;
            foreach (var observer in _observers)
            {
                observer.Key.OnNext(value);
            }
        }
    
        public void Release()
        {
            
            foreach (var observer in _observers)
            {
                observer.Key.OnCompleted();
                observer.Value.Release();
            }
            _observers.Clear();
            
            value = default(T);
            hasValue = false;
        }

        public void Dispose()
        {
            Release();
        }

        protected void Remove(IObserver<T> observer)
        {
            observer.OnCompleted();
            _observers.Remove(observer);
        }
        
    }
}
