namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Runtime.Extensions;

    public class DisposableAction : IDisposableItem
    {
        private Action _onDisposed;

        public bool IsDisposed { get; protected set; } = true;
    
        public void Initialize(Action action)
        {
            IsDisposed = false;
            _onDisposed = action;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            
            _onDisposed?.Invoke();
            
            Release();
        }

        public void Release()
        {
            _onDisposed = null;
        }

        public void MakeDespawn()
        {
            Dispose();
            this.Despawn();
        }
    }
}
