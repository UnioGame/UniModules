namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using DataStructure.LinkedList.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IUniLinkedList<T> : 
        IEnumerator<ListNode<T>>, 
        IPoolable
    {
        ListNode<T> Add(T value);
        void Remove(ListNode<T> node);
    }
}