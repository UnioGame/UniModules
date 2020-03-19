namespace UniModules.UniGame.Core.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Random = UnityEngine.Random;

    public static class CollectionExtension
    {
        public static T GetRandomValue<T>(this IList<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T GetRandomValue<T>(this IList<T> list, Predicate<T> predicate)
        {
            var validValues = list.Where(x=>predicate(x)).ToArray();
            var randomIndex = Random.Range(0, validValues.Length);

            return validValues[randomIndex];
        }

        public static T GetRandomValue<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            var validValues = enumerable.Where(x => predicate(x)).ToArray();
            var randomIndex = Random.Range(0, validValues.Length);

            return validValues[randomIndex];
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
            
            while (result == null || result.Equals(withoutElement)) {
                result = enumerable.GetRandomValue(predicate);
            }
            
            return result;
        }
    }
}