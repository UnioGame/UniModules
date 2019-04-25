using System;
using System.Collections.Generic;

namespace UniModule.UnityTools.UniPool.Scripts
{
    public class MemorizeTool
    {

        public static Func<TKey,TData> Create<TKey,TData>(Func<TKey,TData> factory) {

            var cache = new Dictionary<TKey,TData>();

            return (TKey x) => {

                TData value;
                if (cache.TryGetValue(x, out value) == false) {
                    value = factory(x);
                    cache[x] = value;
                }

                return value;

            };

        }

    }
}
