namespace UniGreenModules.UniCore.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Interfaces.Rx;
    using ObjectPool;
    using ObjectPool.Interfaces;

    public class RecycleReactiveProperty<T> : IRecycleReactiveProperty<T>
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
            
            //if value already exists - notify
            if(hasValue) observer.OnNext(Value);
            
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
            hasValue = false;
            //stop listing all child observers
            foreach (var observer in _observers)
            {
                observer.Key.OnCompleted();
                observer.Value.Release();
            }
            _observers.Clear();
            value = default(T);
        }

        public void MakeDespawn()
        {
            this.Despawn();
        }

        private void Remove(IObserver<T> observer)
        {
            observer.OnCompleted();
            _observers.Remove(observer);
        }

    }
}
