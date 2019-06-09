using UnityEngine;

namespace UniGreenModules.UniCore.Runtime.ScriptableObjects
{
    using System;
    using UniRx;

    public class ScriptableObservable<T> : ScriptableObject,
        IObservable<T>, 
        IDisposable
    {

        protected Subject<T> valueStream = 
            new Subject<T>();

        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            return valueStream.Subscribe(observer);
        }

        public virtual void Dispose()
        {
            valueStream.Dispose();
        }
        
        
    }
}
