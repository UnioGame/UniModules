using System;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.Common
{
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
