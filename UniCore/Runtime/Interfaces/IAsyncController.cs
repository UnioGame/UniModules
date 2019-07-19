namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using DataFlow;
    using UniRx;
    using UniRx.Async;

    public interface IAsyncController : IDisposable
    {
        
        IReadOnlyReactiveProperty<bool> IsInitialized { get; }

        ILifeTime LifeTime { get; }

        UniTask Initialize();
        
    }
}