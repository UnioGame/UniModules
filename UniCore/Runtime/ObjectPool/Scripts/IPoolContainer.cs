using System;

namespace UniModule.UnityTools.UniPool.Scripts
{
    public interface IPoolContainer
    {
        bool Contains(Type type);
        object Pop(Type type);
        void Push(Type type,object item);
    }
}