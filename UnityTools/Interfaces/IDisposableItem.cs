using System;

namespace Assets.Tools.UnityTools.Interfaces
{
    public interface IDisposableItem : IDisposable
    {
        bool IsDisposed { get; }
    }
}
