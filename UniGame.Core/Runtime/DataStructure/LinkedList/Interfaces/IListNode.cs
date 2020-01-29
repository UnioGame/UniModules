namespace UniGreenModules.UniGame.Core.Runtime.DataStructure.LinkedList.Interfaces
{
    using System;
    using Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

    public interface IListNode<T> : 
        IReadonlyValue<T>,
        IDisposable,
        IPoolable
    {
        IListNode<T> Previous { get; set; }
        IListNode<T> Next { get; set; }

    }
}