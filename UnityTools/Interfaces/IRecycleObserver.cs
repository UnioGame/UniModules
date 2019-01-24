using System;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.Interfaces
{
    public interface IRecycleObserver<T> : IObserver<T>, IPoolable
    {

    }
}
