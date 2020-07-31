namespace UniModules.UniGame.Core.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime;
    using UniGreenModules.UniCore.Runtime.ObjectPool.Runtime.Extensions;
    using UniRx;
    using Random = UnityEngine.Random;

    public static class CollectionExtension
    {
        public static T GetRandomValue<T>(this IReadOnlyList<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static IList<T> GetRandomValues<T>(this IList<T> source, int count)
        {
            var result = new List<T>();
            var temp = new List<T>(source);

            for (var i = 0; i < count; i++) {
                var randomIndex = Random.Range(0, temp.Count);
                result.Add(temp[randomIndex]);
                
                temp.RemoveAt(randomIndex);
            }

            return result;
        }

        public static IEnumerable<T> GetRandomValues<T>(this IEnumerable<T> source, int count)
        {
            return GetRandomValues(source.ToList(), count);
        }

        public static T Find<T>(this IReadOnlyList<T> items, Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ApplicationException("NULL predicate");

            for (int index = 0; index < items.Count; ++index) {
                var item = items[index];
                if (predicate(item))
                    return item;
            }
            
            return default (T);
        }
        
        public static int IndexOf<T>(this IReadOnlyList<T> items, T target)
        {
            if (items == null)
                return -1;

            for (int index = 0; index < items.Count; ++index) {
                var item = items[index];
                if (Equals(item, target))
                    return index;
            }

            return -1;
        }
        
        public static int IndexOf<T>(this IReadOnlyList<T> items, Predicate<T> predicate)
        {
            if (items == null)
                return -1;

            for (int index = 0; index < items.Count; ++index) {
                var item = items[index];
                if (predicate(item))
                    return index;
            }

            return -1;
        }

        public static T GetRandomValue<T>(this IReadOnlyList<T> list, Predicate<T> predicate)
        {
            var validValues = list.Where(x=>predicate(x)).ToArray();
            var randomIndex = Random.Range(0, validValues.Length);

            if (randomIndex < 0 || validValues.Length == 0)
                return default;
            
            return validValues[randomIndex];
        }

        public static List<T> AddEnumerableRange<T>(this IEnumerable<T> range, List<T> target)
        {
            target.AddRange(range);
            return target;
        }
        
        public static T GetRandomValue<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            var cache = ClassPool.Spawn<List<T>>();
            
            var validValues = enumerable.
                Where(x => predicate(x)).
                AddEnumerableRange(cache);
            
            var randomIndex = Random.Range(0, cache.Count);

            if (validValues.Count == 0) {
                cache.Despawn();
                return default;
            }
            
            var result = validValues[randomIndex];
            cache.Despawn();
            
            return result;
        }

        public static TValue AddToCollection<TValue, TCollectionValue>(this TValue value, ICollection<TCollectionValue> collection)
            where TValue : TCollectionValue
        {
            collection.Add(value);
            return value;
        }

        public static T GetRandomValue<T>(this IList<T> list, Predicate<T> predicate, T withoutElement)
        {
            var result = default(T);
            
            while (result == null || result.Equals(withoutElement)) {
                result = list.GetRandomValue(predicate);
            }
            
            return result;
        }

        public static T GetRandomValue<T>(this IEnumerable<T> enumerable, Predicate<T> predicate, T withoutElement)
        {
            var result = default(T);

            var items = enumerable.ToList();
            while (result == null || result.Equals(withoutElement)) {
                result = items.GetRandomValue(predicate);
            }
            
            return result;
        }
        
        /// <summary>
        /// Wraps this object instance into an IEnumerable&lt;T&gt;
        /// consisting of a single item.
        /// </summary>
        /// <typeparam name="T"> Type of the object. </typeparam>
        /// <param name="item"> The instance that will be wrapped. </param>
        /// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }
    }
}