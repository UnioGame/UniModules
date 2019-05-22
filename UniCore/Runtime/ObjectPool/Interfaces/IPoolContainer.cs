namespace UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces
{
    using System;

    public interface IPoolContainer
    {
        bool Contains(Type type);
        object Pop(Type type);
        void Push(Type type,object item);
    }
}