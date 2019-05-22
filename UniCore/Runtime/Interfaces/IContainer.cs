namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System.Collections.Generic;
    using ObjectPool.Interfaces;

    public interface IContainer<TData> : IPoolable
    {
        IReadOnlyList<TData> Items { get; }
    }
}