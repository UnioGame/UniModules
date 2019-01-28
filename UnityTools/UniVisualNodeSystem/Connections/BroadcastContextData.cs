using System;
using System.Collections.Generic;
using UniModule.UnityTools.Common;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace UniModule.UnityTools.UniVisualNodeSystem.Connections
{
    
    public class BroadcastContextData<TContext> : IPoolable, IBroadcastContextData<TContext>
    {
        private List<IContextDataWriter<TContext>> _contexts = new List<IContextDataWriter<TContext>>();

        public void Add(IContextDataWriter<TContext> contextData)
        {
            _contexts.Add(contextData);
        }

        public void Remove(IContextDataWriter<TContext> contextData)
        {
            _contexts.Remove(contextData);
        }

        public virtual void Release()
        {
            _contexts.Clear();
        }
        
        #region IContextData interface

        public virtual bool RemoveContext(TContext context)
        {

            for (var i = 0; i < _contexts.Count; i++)
            {
                var item = _contexts[i];
                item.RemoveContext(context);
            }
            
            return true;
        }

        public virtual bool Remove<TData>(TContext context)
        {

            for (int i = 0; i < _contexts.Count; i++)
            {
                var item = _contexts[i];
                item.Remove<TData>(context);
            }
            
            return true;
        }
        
        public virtual void UpdateValue<TData>(TContext targetContext, TData value)
        {
            for (var i = 0; i < _contexts.Count; i++)
            {
                var context = _contexts[i];
                context.UpdateValue<TData>(targetContext,value);
            }
        }
        
        #endregion
    }
    
}
