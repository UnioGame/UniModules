using System;
using UniModule.UnityTools.UniPool.Scripts;

namespace UniModule.UnityTools.Interfaces
{
    public interface IRecycleObserver<T> : IObserver<T>, IPoolable
    {

    }
}
