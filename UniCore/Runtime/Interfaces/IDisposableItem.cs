namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using ObjectPool.Interfaces;

    public interface IDisposableItem : IDisposable,IDespawnable
    {
        bool IsDisposed { get; }
    }
}
