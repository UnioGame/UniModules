namespace UniGreenModules.UniCore.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using ObjectPool;
    using ObjectPool.Extensions;

    public static class CollectionExtension
    {

        public static void RemoveItems<T>(this IEnumerable<T> source,
            Func<T, bool> filter, Action<T> removeAction)
        {
            var removedItems = ClassPool.Spawn<List<T>>();
            
            foreach (var item in source)
            {
                var value = filter(item);
                if (value)
                {
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
