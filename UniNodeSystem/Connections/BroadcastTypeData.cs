
namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    using System;
    using System.Collections.Generic;
    using UniModule.UnityTools.Common;
    using UniModule.UnityTools.ObjectPool.Scripts;

    public class BroadcastTypeData : 
        IPoolable, 
        ITypeDataWriter
    {
        private List<ITypeDataWriter> _registeredItems = new List<ITypeDataWriter>();

        public void Remove(ITypeDataWriter contextData)
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
