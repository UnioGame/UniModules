namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using DataStructure.LinkedList.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public class ListNode<T> : IListNode<T>
    {
        private T value;

        public IListNode<T> Previous { get; set; }
        public IListNode<T> Next     { get; set; }
        
        public T Value => value;
        
        public IListNode<T> SetValue(T target)
        {
            value = target;
            return this;
        }

        public void Dispose()
        {
            this.Despawn();
        }

        public void Release()
        {
            value    = default;
            Previous = null;
            Next     = null;
        }

    }
}