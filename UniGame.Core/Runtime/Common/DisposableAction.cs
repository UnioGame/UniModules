namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Runtime.Extensions;

    public class DisposableAction : IDisposableItem
    {
        private Action _onDisposed;

        public bool IsComplete { get; protected set; } = true;
    
        public void Initialize(Action action)
        {
            IsComplete = false;
            _onDisposed = action;
        }

        public void Dispose()
        {
            if (IsComplete) return;
            IsComplete = true;
            
            _onDisposed?.Invoke();
            
            Complete();
            
            this.Despawn();
        }

        public void Complete()
        {
            IsComplete = true;
            _onDisposed = null;
        }

    }
}
