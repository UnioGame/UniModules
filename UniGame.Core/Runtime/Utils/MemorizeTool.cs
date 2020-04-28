namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;

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
#if UNITY_EDITOR
            //clean up cache if Assembly Reload
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += ClearCache;
#endif      
            return CacheMapFunc;

        }

    }
}
