namespace UniGreenModules.UniCore.Runtime.Interfaces
{
    using System;
    using Cysharp.Threading.Tasks;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniRx;

    public interface IAsyncController : IDisposable
    {
        
        IReadOnlyReactiveProperty<bool> IsInitialized { get; }

        ILifeTime LifeTime { get; }

        UniTask Initialize();
        
    }
}