namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using ObjectPool.Runtime.Interfaces;

    public interface IDisposableItem : 
        IDisposable, 
        ICompletionSource
    {
    }
}
