namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;

    public class MemorizeItem<TKey, TData> : IDisposable
    {
        private readonly Func<TKey, TData> factory;
        private readonly Action<TData>     disposableAction;
        
        private Dictionary<TKey,TData> _cache = new Dictionary<TKey,TData>(16);

        public MemorizeItem(Func<TKey, TData> factory, Action<TData> disposableAction = null)
        {
            this.factory          = factory;
            this.disposableAction = disposableAction;
        }

        public TData this[TKey x] => GetValue(x);

        public TData GetValue(TKey key)
        {
            if (_cache.TryGetValue(key, out var value) == false || value == null) {
                value       = factory(key);
                _cache[key] = value;
            }
            return value;
        }
        
        public void Dispose()
        {
            if (disposableAction != null) {
                foreach (var data in _cache) {
                    disposableAction.Invoke(data.Value);
                }
            }
            _cache.Clear();
        }
    }
}