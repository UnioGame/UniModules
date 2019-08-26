namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;
    using ObjectPool.Interfaces;

    public interface IRecycleObserver<T> : 
        IObserver<T>, 
        IPoolable,
        IDespawnable
    {
        
    }
}
