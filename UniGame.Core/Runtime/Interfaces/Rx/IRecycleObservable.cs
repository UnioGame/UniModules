using System;

namespace UniGreenModules.UniCore.Runtime.Interfaces.Rx
{
    using ObjectPool.Runtime.Interfaces;

    public interface IRecycleObservable<T> :  IObservable<T>,IPoolable
    {

    }
}
