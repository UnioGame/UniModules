namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using DataStructure;
    using global::UniGame.Core.Runtime.Utils;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.DataFlow.Interfaces;
    using UniCore.Runtime.Interfaces.Rx;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniRx;
    using UnityEngine;

    [Serializable]
    public class RecycleReactiveProperty<T> : 
        IRecycleReactiveProperty<T>  
    {
        private IEqualityComparer<T> _equalityComparer;
        private LifeTimeDefinition _lifeTimeDefinition = new LifeTimeDefinition();
        private UniLinkedList<IObserver<T>> _observers;

        [SerializeField]
        protected T value = default;
        
        [Tooltip("Mark this field to true, if you want notify immediately after subscription")]
        [SerializeField]
        protected bool hasValue = false;

        protected UniLinkedList<IObserver<T>> Observers => _observers = _observers ?? new UniLinkedList<IObserver<T>>();

        #region constructor

        public RecycleReactiveProperty(){}

        public RecycleReactiveProperty(T value)
        {
            this.value = value;
            hasValue = true;
        }
        
        #endregion

        public ILifeTime LifeTime => _lifeTimeDefinition ?? (_lifeTimeDefinition = new LifeTimeDefinition());
        
        IEqualityComparer<T> EqualityComparer => _equalityComparer ?? (_equalityComparer = CreateComparer());
        
        public T Value {
            get => value;
            set => SetValue(value);
        }

        public bool HasValue => hasValue;

        #region public methods
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (LifeTime.IsTerminated) {
                observer.OnCompleted();
                return Disposable.Empty;
            }
            
            var disposeAction = ClassPool.
                Spawn<DisposableAction>();
            
            var node = Observers.Add(observer);
            disposeAction.Initialize(() => Remove(node));
              
            //if value already exists - notify
            if(hasValue) observer.OnNext(Value);
            
            return disposeAction;
        }

        public void Dispose() => _lifeTimeDefinition.Terminate();

        public void SetValue(T propertyValue)
        {
            if (hasValue && EqualityComparer.Equals(this.value, propertyValue)) {
                return;
            }

            SetValueForce(propertyValue);
        }

        public void SetValueForce(T propertyValue)
        {
            hasValue = true;
            value    = propertyValue;
            
            do {
                Observers.current?.Value.OnNext(value);
            } while (Observers.MoveNext());

            Observers.Reset();
        }
    
        public void Release()
        {
            CleanUp();
            _lifeTimeDefinition.Release();
        }

        public Type Type => typeof(T);

        public object GetValue() => value;
        
        public void SetObjectValue(object value)
        {
            if (value is T targetValue) {
                SetValue(targetValue);
            }
        }
        
        #endregion
        
        protected virtual IEqualityComparer<T> CreateComparer() => UnityEqualityComparer.GetDefault<T>();

        protected virtual void OnRelease(){}
        
        private void Remove(ListNode<IObserver<T>> observer)
        {
            if (!LifeTime.IsTerminated)
                observer.Value?.OnCompleted();
            Observers.Remove(observer);
        }

        private void CleanUp()
        {
            value = default;
            hasValue = false;
            
            //stop listing all child observers
            do {
                Observers.current?.Value.OnCompleted();
            } while (Observers.MoveNext());
            
            Observers.Release();
            value = default(T);
                     
            OnRelease();
        }

    }
}
