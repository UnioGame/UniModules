namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using Interfaces;

    public class DummyPoolContainer : IPoolContainer
    {

        public bool Contains(Type type)
        {
            return false;
        }

        public object Pop(Type type)
        {
            return null;
        }

        public void Push(Type type, object item)
        {
            
        }
    }
}
