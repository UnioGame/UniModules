namespace UniGreenModules.UniCore.Runtime.Utils
{
    using System;
    using System.Collections.Generic;

    public class MemorizeTool
    {

        public static Func<TKey,TData> Create<TKey,TData>(Func<TKey,TData> factory) {

            var cache = new Dictionary<TKey,TData>(16);

            return x => {

                if (cache.TryGetValue(x, out var value) == false || value == null) {
                    value = factory(x);
                    cache[x] = value;
                }

                return value;

            };

        }

    }
}
