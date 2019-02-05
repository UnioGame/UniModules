using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using System.Collections.Generic;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    public abstract class ContextConnection<TContext> : IContextDataWriter<TContext>
    {
        protected IContextData<TContext> _target;
        protected List<IContextData<TContext>> _connections = new List<IContextData<TContext>>();
        
        public ContextConnection(IContextData<TContext> target)
        {
            _target = target;
        }

        public void Connect(IContextData<TContext> connectedContext)
        {
            _connections.Add(connectedContext);
        }

        public void Disconnect(IContextData<TContext> connectedContext)
        {
            _connections.Remove(connectedContext);
        }

        public abstract void UpdateValue<TData>(TContext context, TData value);

        public abstract bool RemoveContext(TContext context);

        public abstract bool Remove<TData>(TContext context);
    }
}
