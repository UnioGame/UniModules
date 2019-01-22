using System;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace UnityTools.Interfaces
{
    public interface IRecycleObserver<T> : IObserver<T>, IPoolable
    {

    }
}
