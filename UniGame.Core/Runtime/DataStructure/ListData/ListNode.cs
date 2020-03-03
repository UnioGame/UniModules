namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;

    public class ListNode<T>// : IListNode<T>
    {
        public T Value;
        public ListNode<T> Previous;
        public ListNode<T> Next;
        
        public ListNode<T> SetValue(T target)
        {
            Value = target;
            return this;
        }

        public void Dispose()
        {
            Release();
            this.Despawn();
        }

        public void Release()
        {
            Value    = default;
            Previous = null;
            Next     = null;
        }

    }
}