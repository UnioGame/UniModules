
namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    using System;
    using System.Collections.Generic;
    using UniModule.UnityTools.Common;
    using UniModule.UnityTools.ObjectPool.Scripts;

    public class BroadcastTypeData : 
        IPoolable, 
        IContextWriter,
        IConnector<IContextWriter>
    {
        private List<IContextWriter> _registeredItems = new List<IContextWriter>();

        public void Remove(IContextWriter contextData)
        {
            _registeredItems.Remove(contextData);
        }

        public virtual void Release()
        {
            _registeredItems.Clear();
        }
        
        #region IContextData interface

        public virtual bool Remove<TData>()
        {
            
            for (var i = 0; i < _registeredItems.Count; i++)
            {
                var item = _registeredItems[i];
                item.Remove<TData>();
            }
            
            return true;          
        }

        public virtual void Add<TData>(TData value)
        {
            for (var i = 0; i < _registeredItems.Count; i++)
            {
                var context = _registeredItems[i];
                context.Add(value);
            }
        }

        #endregion

        public IConnector<IContextWriter> Connect(IContextWriter connection)
        {
            _registeredItems.Add(connection);
            return this;
        }

        public void Disconnect(IContextWriter connection)
        {
            _registeredItems.Remove(connection);
        }
        
    }
    
}
