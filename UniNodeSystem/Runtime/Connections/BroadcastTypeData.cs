
namespace UniGreenModules.UniNodeSystem.Runtime.Connections
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool.Interfaces;

    public class BroadcastTypeData : 
        IPoolable, 
        IContextWriter,
        IConnector<IContextWriter>
    {
        private List<IContextWriter> _registeredItems = new List<IContextWriter>();

        #region ipoolable
        
        public virtual void Release()
        {
            _registeredItems.Clear();
        }
        
        #endregion
        
        #region IContextData interface

        public void CleanUp()
        {
            BroadcastAction(x => x.CleanUp());
        }
        
        public virtual bool Remove<TData>()
        {
            BroadcastAction(x => x.Remove<TData>());
            return true;          
        }

        public void Add<TData>(TData value)
        {
            BroadcastAction(x => x.Add(value));
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

        private void BroadcastAction(Action<IContextWriter> action)
        {
            for (var i = 0; i < _registeredItems.Count; i++)
            {
                var context = _registeredItems[i];
                action(context);
            }
        }
        
    }
    
}
