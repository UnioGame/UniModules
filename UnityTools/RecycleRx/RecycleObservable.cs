using System;
using System.Collections.Generic;
using Assets.Tools.UnityTools.Common;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace UnityTools.RecycleRx
{
    public class RecycleObservable<T> : IObservable<T>,IPoolable
    {
        
        private Dictionary<IObserver<T>,DisposableAction> _observers = 
            new Dictionary<IObserver<T>, DisposableAction>();
    
        public T Value { get; protected set; }
    
        public IDisposable Subscribe(IObserver<T> observer)
        {
            
            var disposeAction = ClassPool.Spawn<DisposableAction>();
            disposeAction.Initialize(() => _observers.Remove(observer));

            _observers[observer] = disposeAction;
            
            observer.OnNext(Value);
            
            return disposeAction;

        }

        public void SetValue(T value)
        {
            Value = value;
            foreach (var observer in _observers)
            {
                observer.Key.OnNext(value);
            }
        }
    
        public void Release()
        {
            Value = default(T);
            foreach (var observer in _observers)
            {
                observer.Key.OnCompleted();
                observer.Value.Release();
            }
            _observers.Clear();
        }

        public void Dispose()
        {
            Release();
        }

    }
}
