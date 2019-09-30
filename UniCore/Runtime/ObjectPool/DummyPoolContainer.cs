namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using Interfaces;

    public class DummyPoolContainer : IPoolContainer
    {

        public bool Contains<T>() where T : class => false;

        public T Pop<T>() where T : class => null;

        public void Push<T>(T item) where T : class
        {
        }
    }
}
