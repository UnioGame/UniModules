using System;

namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Interfaces;

    public interface IRecycleObservable<T> :  IObservable<T>,IPoolable
    {

    }
}
