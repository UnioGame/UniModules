namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface IDisposableItem : IDisposable
    {
        bool IsDisposed { get; }
    }
}
