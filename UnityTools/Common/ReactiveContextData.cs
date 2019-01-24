using System;
using System.Collections.Generic;
using UniModule.UnityTools.Interfaces;
using UniModule.UnityTools.ObjectPool.Scripts;
using UniRx;

namespace UniModule.UnityTools.Common
{
    public class ReactiveContextData<TContext> : 
        IContextData<TContext>, 
        IPoolable
    {
        private ContextData<TContext> _contextData = new ContextData<TContext>();

        #region reactive methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="observer"></param>
        /// <returns></returns>
        public IDisposable SubscribeOnContextChanged(Action<TContext> observer)
        {
            return Disposable.Empty;
        }

        public IDisposable SubscribeOnContextRemoved(IObserver<TContext> observer)
        {
            return Disposable.Empty;
        }
        
        
        #endregion

        #region IContextData<TContext>

        public IReadOnlyList<TContext> Contexts => _contextData.Contexts;
        public int Count => _contextData.Count;

        public void UpdateValue<TData>(TContext context, TData value)
        {
            _contextData.UpdateValue(context,value);
        }

        public bool HasValue(TContext context, Type type)
        {
            return _contextData.HasValue(context, type);
        }

        public bool HasValue<TValue>(TContext context)
        {
            return _contextData.HasValue<TValue>(context);
        }

        public bool HasContext(TContext context)
        {
            return _contextData.HasContext(context);
        }

        public TData Get<TData>(TContext context)
        {
            return _contextData.Get<TData>(context);
        }

        public bool RemoveContext(TContext context)
        {
            return _contextData.RemoveContext(context);
        }

        public bool Remove<TData>(TContext context)
        {
            return Remove<TData>(context);
        }

        public void Release()
        {
            _contextData.Release();
        }
        
        public void CopyTo(TContext context, IMessagePublisher writer)
        {
            _contextData.CopyTo(context,writer);
        }
        
        #endregion

    }
}
