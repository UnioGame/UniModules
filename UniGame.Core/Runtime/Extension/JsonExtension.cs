namespace UniModules.UniGame.Core.Runtime.Extension
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public static class JsonExtension
    {
        public static T[] FromArrayJson<T>(this string json)
        {
            var wrapper = JsonUtility.FromJson<JsonArrayValue<T>>(json);
            return wrapper.Items;
        }
    
        public static List<T> FromListJson<T>(this string json)
        {
            var wrapper = JsonUtility.FromJson<JsonListValue<T>>(json);
            return wrapper.Items;
        }

        public static T FromJson<T>(this string value)
        {
            return JsonUtility.FromJson<T>(value);
        }
    
        public static object FromJson(this string value,Type type)
        {
            return JsonUtility.FromJson(value,type);
        }
    
        public static string ToJson<T>(this T value)
        {
            return JsonUtility.ToJson(value);
        }
    
        public static string ToJson<T>(this T[] array)
        {
            var wrapper = new JsonArrayValue<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(this List<T> array, bool prettyPrint)
        {
            var wrapper = new JsonListValue<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

    }

    [Serializable]
    public class JsonArrayValue<T>
    {
        public T[] Items;
    }

    [Serializable]
    public class JsonListValue<T>
    {
        public List<T> Items;
    }
}