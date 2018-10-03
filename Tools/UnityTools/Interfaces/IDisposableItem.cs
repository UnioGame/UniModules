using System;

namespace Assets.Modules.UnityToolsModule.Tools.UnityTools.Interfaces
{
    public interface IDisposableItem : IDisposable
    {
        bool IsDisposed { get; }
    }
}
