namespace UniGreenModules.UniContextData.Runtime
{
    using System;
    using System.Collections.Generic;
    using UniCore.Runtime.Common;
    using UniCore.Runtime.Interfaces;
    using UniCore.Runtime.ObjectPool;
    using UniCore.Runtime.ObjectPool.Extensions;
    using UniCore.Runtime.ObjectPool.Interfaces;

    public class ContextData<TContext> :
        IContextData<TContext>,
        IPoolable
    {
        protected List<TContext>                 _contextsItems = new List<TContext>();
        protected Dictionary<TContext, TypeData> _contexts      = new Dictionary<TContext, TypeData>();

        public IReadOnlyList<TContext> Contexts => _contextsItems;

        public int Count => _contexts.Count;

#region public methods

        public virtual void UpdateValue<TData>(TContext context, TData value)
        {
            var container = GetTypeData(context, true);
            container.Add(value);
        }

        public virtual bool Remove<TData>(TContext context)
        {
            var container = GetTypeData(context);
            if (container == null)
                return false;
            return container.Remove<TData>();
        }

        public virtual bool RemoveContext(TContext context)
        {
            if (_contexts.TryGetValue(context, out var contextData)) {
                contextData.Despawn();
                _contexts.Remove(context);
                _contextsItems.Remove(context);
                return true;
            }

            return false;
        }

        public TData Get<TData>(TContext context)
        {
            var container = GetTypeData(context);
            return container.Get<TData>();
        }

        public bool HasContext(TContext context)
        {
            return context != null && _contexts.ContainsKey(context);
        }

        public bool HasValue<TValue>(TContext context)
        {
            return HasValue(context, typeof(TValue));
        }

        public bool HasValue(TContext context, Type type)
        {
            var container = GetTypeData(context);
            return container != null && container.Contains(type);
        }

        public void Release()
        {
            var contexts = ClassPool.Spawn<List<TContext>>();
            contexts.AddRange(_contexts.Keys);

            foreach (var contextData in contexts) {
                RemoveContext(contextData);
            }

            contexts.DespawnCollection();

            _contexts.Clear();
            _contextsItems.Clear();

            OnRelease();
        }

#endregion

        protected TypeData GetTypeData(TContext context, bool createIfEmpty = false)
        {
            if (!_contexts.TryGetValue(context, out var contextData) && createIfEmpty) {
                contextData        = ClassPool.Spawn<TypeData>();
                _contexts[context] = contextData;
                _contextsItems.Add(context);
            }

            return contextData;
        }

        protected virtual void OnRelease()
        {
        }
    }
}