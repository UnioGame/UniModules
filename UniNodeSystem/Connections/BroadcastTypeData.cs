using System;
using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    
    public class BroadcastTypeData : 
        IPoolable, 
        IBroadcastTypeData, 
        ITypeDataWriter
    {
        private List<ITypeDataContainer> _registeredItems = new List<ITypeDataContainer>();

        public void Connect(ITypeDataContainer contextData)
        {
            _registeredItems.Add(contextData);
        }

        public void Remove(ITypeDataContainer contextData)
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
            return Remove(typeof(TData));            
        }

        public bool Remove(Type type)
        {
            for (var i = 0; i < _registeredItems.Count; i++)
            {
                var item = _registeredItems[i];
                item.Remove(type);
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
    }
    
}
