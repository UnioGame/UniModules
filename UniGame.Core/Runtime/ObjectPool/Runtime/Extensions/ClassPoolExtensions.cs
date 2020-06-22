namespace UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public static class ClassPoolExtensions
    {
        public static void Despawn<T>(this T source, ref T instance, T defaultValue = null) where T : class
        {
            if(source == null || !source.Equals(instance))
                return;
            
            source.Despawn();
            
            instance = defaultValue;
        }

        public static void Despawn<T>(this T data)
            where T : class
        {
            if (data == null) return;
            ClassPool.Despawn(data);
        }
        
        public static void DespawnObject<T>(this T data, Action cleanupAction)
            where T: class
        {
            if (data == null) return;
            cleanupAction?.Invoke();
            ClassPool.Despawn(data);
        }
        
        public static void DespawnRecursive<TValue,TData>(this TValue data)
            where TValue : class, ICollection<TData> 
        {
            DespawnItems(data);
            DespawnCollection<TValue,TData>(data);
        }
        
        public static void Despawn<TData>(this List<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this HashSet<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this Stack<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TKey,TValue>(this Dictionary<TKey,TValue> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void Despawn<TData>(this Queue<TData> value)
        {
            value.Clear();
            ClassPool.Despawn(value);
        }
        
        public static void DespawnCollection<TValue,TData>(this TValue value)
            where  TValue : class, ICollection<TData> 
        {
            value.Clear();
            ClassPool.Despawn(value);
        }

        public static void DespawnDictionary<TData,TKey,TValue>(this TData data)
            where TData : class, IDictionary<TKey,TValue>
        {
            data.Clear();
            ClassPool.Despawn(data);
        }

        public static void DespawnItems<TData>(this ICollection<TData> data)
        {
            foreach (var item in data) {
                if (item is IPoolable poolable)
                    poolable.Despawn();
            }
            data.Clear();
        }

    }
}
