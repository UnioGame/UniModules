namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;

    public interface IAsyncController : IDisposable
    {
        
        bool Complete { get; }
        bool Pause();
        bool Start();
        
    }
}