namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using ObjectPool;
    using ObjectPool.Runtime;
    using ObjectPool.Runtime.Extensions;

    public static class CollectionExtension
    {

        public static void RemoveItems<T>(this IReadOnlyList<T> source,
            Func<T, bool> filter, Action<T> removeAction)
        {
            var removedItems = ClassPool.Spawn<List<T>>();

            for (var i = 0; i < source.Count; i++) {
                var item  = source[i];
                var value = filter(item);
                if (value) {
                    removedItems.Add(item);
                }
            }

            foreach (var removedItem in removedItems)
            {
                removeAction(removedItem);
            }

            removedItems.DespawnCollection();
        }
        
    }
}
