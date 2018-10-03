using System.Collections.Generic;
using Assets.Tools.UnityTools.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.Common
{
    public class ContextProviderProvider<TContext> : IContextProvider<TContext>
    {
        private Dictionary<TContext, ContextData> _contexts = new Dictionary<TContext, ContextData>();

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

        public void RemoveContext(TContext context)
        {

            if (_contexts.TryGetValue(context, out var contextData) == false)
            {
                contextData.Despawn();
                _contexts.Remove(context);
            }

        }

        public void Remove<TData>(TContext context)
        {
            if (_contexts.TryGetValue(context, out var contextData) == false)
            {
                contextData.Remove<TData>();
            }
        }

        public void Release()
        {
            foreach (var contextData in _contexts)
            {
                contextData.Value.Despawn();
            }
            _contexts.Clear();
        }
    }
}
