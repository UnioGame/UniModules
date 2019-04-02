using System;

namespace UniModule.UnityTools.Interfaces
{
    public interface IDisposableItem : IDisposable
    {
        bool IsDisposed { get; }
    }
}
