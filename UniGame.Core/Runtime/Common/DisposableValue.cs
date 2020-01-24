namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using ObjectPool;

    public struct DisposableValue
    {
        private Action onDisposeAction;

        public bool IsDisposed;
    
        public DisposableValue(Action action)
        {
            IsDisposed = false;
            onDisposeAction = action;
        }

        public void Dispose()
        {
            if (IsDisposed) return;
            IsDisposed = true;
            onDisposeAction?.Invoke();
            Release();
        }

        public void Release()
        {
            onDisposeAction = null;
        }
        
    }
}
