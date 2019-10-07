namespace UniGreenModules.UniCore.Runtime.ObjectPool.Extensions
{
    using System.Collections.Generic;
    using Interfaces;

    public static class ClassPoolExtensions
    {

        public static void DespawnRecursive<TData>(this IList<TData> data)
            where TData : class
        {
            DespawnItems(data);
            data.DespawnCollection();
        }
        
        public static void DespawnCollection<TData>(this ICollection<TData> data)
        {
            data.Clear();
            data.Despawn();
        }


        public static void DespawnDictionary<TKey,TData>(this IDictionary<TKey,TData> data)
            where TData : class
        {
            data.Clear();
            data.Despawn();
        }

        public static void DespawnItems<TData>(this IList<TData> data)
            where TData : class 
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] is IPoolable)
                    data[i].Despawn();
            }
            data.Clear();
        }

    }
}
