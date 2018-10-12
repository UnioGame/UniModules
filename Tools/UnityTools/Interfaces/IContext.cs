using System;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IContext : IPoolable
    {

        TData Get<TData>();
        bool Remove<TData>();
        void Add<TData>(TData data);

        IObservable<TData> Observable<TData>();

    }
}
