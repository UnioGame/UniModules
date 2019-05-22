namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using ObjectPool.Interfaces;

    public interface IRecycleObserver<T> : IObserver<T>, IPoolable
    {

    }
}
