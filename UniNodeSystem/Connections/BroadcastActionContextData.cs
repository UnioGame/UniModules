using System;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace Modules.UniTools.UniNodeSystem.Connections
{
    public class BroadcastActionContextData<TContext> : 
        IDisposable, 
        IPoolable, 
        ITypeDataWriter
    {
        private Action<TContext> _onRemoveDataAction;
        private Action<TContext> _onUpdate;
        private Action<TContext> _onRemoveContextAction;
    
        public void Initialize(Action<TContext> onUpdate,
            Action<TContext> onRemoveData = null,
            Action<TContext> onRemoveContext = null)
        {
            Release();
            _onUpdate = onUpdate;
            _onRemoveDataAction = onRemoveData;
            _onRemoveContextAction = onRemoveContext;
        }

        public void Release()
        {
            _onUpdate = null;
            _onRemoveContextAction = null;
            _onRemoveDataAction = null;
        }

        public void Add<TData>(TData value)
        {
            _onUpdate?.Invoke(value);
        }

        public bool RemoveContext(TContext context)
        {
            _onRemoveContextAction?.Invoke(context);
            return true;
        }

        public bool Remove<TData>(TContext context)
        {
            _onRemoveDataAction?.Invoke(context);
            return true;
        }

        public void Dispose()
        {
            this.Despawn();
        }
    }
}
