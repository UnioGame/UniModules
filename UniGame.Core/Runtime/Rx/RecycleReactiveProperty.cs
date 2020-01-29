namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using System;
    using DataStructure.LinkedList;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces.Rx;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;

    public class RecycleReactiveProperty<T> : IRecycleReactiveProperty<T>
    {
        private T value = default;
        private bool hasValue;
        
        private UniLinkedList<IObserver<T>> observers = new UniLinkedList<IObserver<T>>();

        public T Value {
            get => value;
            set => SetValue(value);
        }

        public bool HasValue => hasValue;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            var disposeAction = ClassPool.Spawn<DisposableAction>();
            var node = observers.Add(observer);
            disposeAction.Initialize(() => Remove(node));
              
            //if value already exists - notify
            if(hasValue) observer.OnNext(Value);
            
            return disposeAction;
        }

        public void SetValue(T propertyValue)
        {
            hasValue = true;
            value = propertyValue;
            
            do {
                observers.current?.Value.OnNext(value);
            } while (observers.MoveNext());

            observers.Reset();
        }
    
        public void Release()
        {
            hasValue = false;
            
            //stop listing all child observers
            do {
                observers.current?.Value.OnCompleted();
            } while (observers.MoveNext());
            
            observers.Release();
            value = default(T);
        }

        public void MakeDespawn()
        {
            this.Despawn();
        }

        private void Remove(ListNode<IObserver<T>> observer)
        {
            observer.Value.OnCompleted();
            observers.Remove(observer);
        }

    }
}
