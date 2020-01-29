namespace UniGreenModules.UniGame.Core.Runtime.DataStructure.LinkedList
{
    using System;
    using System.Collections;
    using Interfaces;
    using Rx;
    using UniCore.Runtime.ObjectPool.Runtime;

    /// <summary>
    /// Lightweight property broker.
    /// </summary>
    [Serializable]
    public class UniLinkedList<T> : IUniLinkedList<T>
    {
        public ListNode<T> current;
        
        [NonSerialized]
        public ListNode<T> root;

        [NonSerialized]
        public ListNode<T> last;

        public ListNode<T> Add(T value)
        {
            var node = ClassPool.Spawn<ListNode<T>>();
            
            // subscribe node, node as subscription.
            var next = node.SetValue(value);
            if (root == null)
            {
                current = root = last = next;
            }
            else
            {
                last.Next     = next;
                next.Previous = last;
                last          = next;
            }
            
            return next;
        }

        public void Remove(ListNode<T> node)
        {
            if (node == root)
            {
                root = node.Next;
            }
            if (node == last)
            {
                last = node.Previous;
            }

            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        public void Dispose()
        {
            Release();
        }

        public void Release()
        {
            Reset();
            var node          = root;
            current = root = last = null;

            while (node != null)
            {
                var next = node.Next;
                node.Dispose();
                node = next;
            }
        }
        
        #region ienumerator

        public bool MoveNext()
        {
            if (current == null) return false;
            current = current.Next;
            return current != null;
        }

        public void Reset()
        {
            current = root;
        }

        public ListNode<T> Current => current;

        object IEnumerator.Current => Current;
        
        #endregion
    }
}
