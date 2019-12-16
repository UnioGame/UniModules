namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using System;
    using ObjectPool.Runtime.Interfaces;

    public interface IRecycleObserver<T> : 
        IObserver<T>, 
        IPoolable,
        IDespawnable
    {
        
    }
}
