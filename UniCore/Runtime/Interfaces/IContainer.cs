namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System.Collections.Generic;
    using ObjectPool.Runtime.Interfaces;

    public interface IContainer<TData> : IPoolable
    {
        IReadOnlyList<TData> Items { get; }
    }
}