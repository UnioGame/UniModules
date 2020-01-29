namespace UniGreenModules.UniGame.Core.Runtime.Rx
{
    using System;
    using System.Collections.Generic;
    using DataStructure.LinkedList.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IUniLinkedList<T> : 
        IEnumerator<IListNode<T>>, 
        IPoolable
    {
        IListNode<T> Add(T value);
        void Remove(IListNode<T> node);
    }
}