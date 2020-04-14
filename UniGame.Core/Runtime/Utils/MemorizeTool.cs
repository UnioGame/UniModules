namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;

    public class MemorizeTool
    {
        public static Func<TKey,TData> Create<TKey,TData>(Func<TKey,TData> factory) {

            var cache = new Dictionary<TKey,TData>(16);

            TData CacheMapFunc(TKey x)
            {
                if (cache.TryGetValue(x, out var value) == false || value == null) {
                    value    = factory(x);
                    cache[x] = value;
                }
                return value;
            }

            void ClearCache() => cache.Clear();

            //clean up cache if Assembly Reload
            AssemblyReloadEvents.beforeAssemblyReload += ClearCache;
            
            return CacheMapFunc;

        }

    }
}
