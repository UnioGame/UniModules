using System;

namespace Assets.Tools.UnityTools.Common
{
    public class DisposableAction : IDisposable
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
            _onDisposed?.Invoke();
            _onDisposed = null;
        
        }
    }
}
