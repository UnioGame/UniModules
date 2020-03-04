namespace UniGreenModules.UniCore.Runtime.Common
{
    using System;
    using Interfaces;
    using ObjectPool;
    using ObjectPool.Runtime.Extensions;

    public class DisposableAction<TArg> : IDisposableItem
    {
        private Action<TArg> _onDisposed;
        private TArg _arg;
    
        public bool IsCompleted { get; protected set; }
    
        public void Initialize(Action<TArg> action, TArg arg)
        {
            IsCompleted = false;
            _onDisposed = action;
            _arg = arg;
        }

        public void Reset()
        {
            IsCompleted = true;
            _onDisposed = null;
            _arg = default(TArg);
        }
    
        public void Dispose()
        {

            if (!IsCompleted)
            {
                _onDisposed?.Invoke(_arg);
            }
            
            Reset();
        
            //return to pool
            //this.Despawn();
        
        }

        public void MakeDespawn()
        {
            Dispose();
            this.Despawn();
        }
    }
}
