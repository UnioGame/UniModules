using System.Collections.Generic;
using Assets.Modules.UnityToolsModule.Tools.UnityTools.DataFlow;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Common
{
    public class ContextDataProvider<TContext> : IContextData<TContext>
    {

        private Dictionary<TContext, ContextData> _contexts = new Dictionary<TContext, ContextData>();

        public IReadOnlyCollection<TContext> Contexts => _contexts.Keys;

        public TData Get<TData>(TContext context)
        {

            if (_contexts.TryGetValue(context, out var contextData))
            {
                return contextData.Get<TData>();
            }
            return default(TData);

        }

        public void AddValue<TData>(TContext context, TData value)
        {

            if (_contexts.TryGetValue(context, out var contextData) == false)
            {
                contextData = ClassPool.Spawn<ContextData>();
                _contexts[context] = contextData;
            }

            contextData.Add(value);

        }

        public bool HasContext(TContext context)
        {
            return context != null && _contexts.ContainsKey(context);
        }

        public void RemoveContext(TContext context)
        {

            if (_contexts.TryGetValue(context, out var contextData))
            {
                contextData.Despawn();
                _contexts.Remove(context);
            }

        }

        public void Remove<TData>(TContext context)
        {
            if (_contexts.TryGetValue(context, out var contextData))
            {
                contextData.Remove<TData>();
            }
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


    }
}
