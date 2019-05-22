namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using ObjectPool;
    using ObjectPool.Interfaces;

    public class DisposableAction : IDisposable, IPoolable
    {
        private Action _onDisposed;
    
        public bool IsDisposed { get; protected set; }
    
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
        
            ClassPool.Despawn(this);
        }

        public void Release()
        {
            _onDisposed = null;
        }
    }
}
