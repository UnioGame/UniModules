using System;
using System.Collections.Generic;
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
        ICopyableData<TContext>,
        IPoolable
    {

        private Dictionary<TContext, TypeData> _contexts = new Dictionary<TContext, TypeData>();

        public IReadOnlyCollection<TContext> Contexts => _contexts.Keys;

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
            
        }

        public void CopyTo(TContext context, IMessagePublisher target)
        {
            
            if (!_contexts.TryGetValue(context, out var contextData))
            {
                return;
            }

            foreach (var value in contextData.Values)
            {
                value.CopyTo(target);
            }
            
        }
        
        #endregion

        protected TypeData GetTypeData(TContext context)
        {
            if (!_contexts.TryGetValue(context, out var contextData))
            {
                contextData = ClassPool.Spawn<TypeData>();
                _contexts[context] = contextData;
            }

            return contextData;
        }
        
    }
}
