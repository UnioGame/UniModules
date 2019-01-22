using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UniRx;
using UnityTools.Common;
using UnityTools.Interfaces;

namespace Assets.Tools.UnityTools.Common
{

    public class ContextData<TContext> : 
        IContextData<TContext>, 
        IPoolable
    {
        private List<TContext> _contextsItems = new List<TContext>();
        protected Dictionary<TContext, TypeData> _contexts = new Dictionary<TContext, TypeData>();

        public IList<TContext> Contexts => _contextsItems;

        public int Count => _contexts.Count;
        
        #region public methods

        public TData Get<TData>(TContext context)
        {
            var container = GetTypeData(context);
            return container.Get<TData>();
        }

        public void UpdateValue<TData>(TContext context, TData value)
        {
            
            var container = GetTypeData(context);
            container.Add<TData>(value);

        }
        
        public bool HasContext(TContext context)
        {
            return context != null && _contexts.ContainsKey(context);
        }

        public bool RemoveContext(TContext context)
        {

            if (_contexts.TryGetValue(context, out var contextData))
            {
                contextData.Despawn();
                _contexts.Remove(context);
                _contextsItems.Remove(context);
                return true;
            }

            return false;
        }

        public bool HasValue<TValue>(TContext context)
        {
            
            var container = GetTypeData(context);
            return container.HasData<TValue>();
            
        }

        public bool HasValue(TContext context,Type type)
        {
            var container = GetTypeData(context);
            return container.HasData(type);
        }

        public bool Remove<TData>(TContext context)
        {
            
            var container = GetTypeData(context);
            return container.Remove<TData>();

        }

        public void Release()
        {
            
            var contexts = ClassPool.Spawn<List<TContext>>();
            contexts.AddRange(_contexts.Keys);
            
            foreach (var contextData in contexts)
            {
                RemoveContext(contextData);
            }
            
            contexts.DespawnCollection();
            
            _contexts.Clear();
            _contextsItems.Clear();
            
        }

        public void CopyTo(TContext context, IMessagePublisher target)
        {
            
            if (!_contexts.TryGetValue(context, out var contextData))
            {
                return;
            }

            var items = contextData.WritableItems;
            for (int i = 0; i < items.Count(); i++)
            {
                var item = items[i];
                item.CopyTo(target);
            }
            
        }
        
        #endregion

        protected TypeData GetTypeData(TContext context)
        {
            if (!_contexts.TryGetValue(context, out var contextData))
            {
                contextData = ClassPool.Spawn<TypeData>();
                _contexts[context] = contextData;
                _contextsItems.Add(context);
            }

            return contextData;
        }
        
    }
}
