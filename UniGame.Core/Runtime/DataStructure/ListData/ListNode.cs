namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using System;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public class ListNode<T> : IPoolable
    {
        public T Value;
        public ListNode<T> Previous;
        public ListNode<T> Next;
        
        public ListNode<T> SetValue(T target)
        {
            Value = target;
            return this;
        }

        public void Dispose() => this.Despawn();

        public void Release()
        {
            Value    = default;
            Previous = null;
            Next     = null;
        }

    }
}