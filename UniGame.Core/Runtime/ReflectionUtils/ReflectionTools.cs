namespace UniGreenModules.UniCore.Runtime.ReflectionUtils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DataStructure;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class ReflectionTools
    {
        private static Type _stringType = typeof(string);

        public const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

        public static DoubleKeyDictionary<Type,string,FieldInfo> fieldInfos = new DoubleKeyDictionary<Type,string,FieldInfo>();

        public static void Clear()
        {
            fieldInfos.Clear();
        }

        public static FieldInfo GetFieldInfoCached(this object target,string name) => GetFieldInfoCached(target.GetType(),name);
        
        public static FieldInfo GetFieldInfoCached<T>(string name) => GetFieldInfoCached(typeof(T),name);
        
        public static FieldInfo GetFieldInfoCached(this Type type,string name)
        {
            var info = fieldInfos.Get(type, name);
            if (info != null) return info;
            info = type.GetField(name,bindingFlags);

            if (info == null) return null;
            
            fieldInfos.Add(type,name,info);
            return info;
        }
        
        public static void SearchInFieldsRecursively<T>(object target, Object parent, Action<Object, T> onFoundAction, HashSet<object> validatedObjects, Func<T, T> resourceAction = null)
        {
            if (target == null || !validatedObjects.Add(target)) return;

            var targetType = target.GetType();
            var fields = targetType.GetFields();
            foreach (var fieldInfo in fields)
            {

                SearchInObject<T>(target, parent, fieldInfo, onFoundAction, validatedObjects, resourceAction);

            }
        }

        private static void SearchInObject<T>(object target, Object parent, FieldInfo fieldInfo, Action<Object, T> onFoundAction, HashSet<object> validatedObjects, Func<T, T> resourceAction)
        {

            try
            {

                if (target == null) return;

                var searchType = typeof(T);
                var item = fieldInfo.GetValue(target);

                if (Validate(item, searchType) == false)
                    return;

                T resultItem;
                if (ProcessItem<T>(target, fieldInfo, item, out resultItem, resourceAction))
                {
                    if (onFoundAction != null) onFoundAction(parent, resultItem);
                    return;
                }

                var collection = item as ICollection;
                if (collection != null)
                {
                    validatedObjects.Add(collection);
                    SearchInCollection(target, parent, collection, onFoundAction, validatedObjects, resourceAction);
                    return;
                }

                var assetItem = item as Object;
                parent = assetItem == null ? parent : assetItem;

                SearchInFieldsRecursively(item, parent, onFoundAction, validatedObjects);

            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        private static void SearchInCollection<T>(object target, Object parent, ICollection collection, Action<Object, T> onFoundAction, HashSet<object> validatedObjects, Func<T, T> resourceAction)
        {

            if (collection.Count > 0)
            {
                var searchingType = typeof(T);
                foreach (var collectionItem in collection)
                {
                    if (collectionItem == null || Validate(collectionItem.GetType(), searchingType) == false)
                        continue;
                    T resultItem;

                    if (ProcessItem<T>(target, null, collectionItem, out resultItem, resourceAction))
                    {
                        if (onFoundAction != null) onFoundAction(parent, resultItem);
                        continue;
                    }

                    var assetItem = collectionItem as Object;
                    parent = assetItem == null ? parent : assetItem;
                    SearchInFieldsRecursively(collectionItem, parent, onFoundAction, validatedObjects);
                }
            }
        }

        private static bool ProcessItem<T>(object target, FieldInfo fieldInfo, object item, out T result, Func<T, T> resourceAction)
        {

            var resultItem = default(T);
            var searchingType = typeof(T);

            result = resultItem;

            if (item == null || searchingType.IsInstanceOfType(item) == false) return false;

            result = (T)item;
            if (resourceAction != null)
            {
                result = resourceAction(result);
                if (fieldInfo != null)
                    fieldInfo.SetValue(target, result);
            }

            return true;

        }

        public static List<Type> GetDerivedTypes(Type aType) {
            
            var  appDomain = System.AppDomain.CurrentDomain;
            var result = new List<System.Type>();
            var assemblies = appDomain.GetAssemblies();
            
            for(int i = 0; i<assemblies.Length; i++)
            {
                var assembly = assemblies[i];
                var types = assembly.GetTypes();
                for (var j = 0; j < types.Length; j++) {
                    var type = types[j];
                    if (type.IsSubclassOf(aType))
                        result.Add(type);
                }
            }
            return result;
        }
        
        public static bool Validate(object item, Type searchType)
        {
            if (item == null)
                return false;

            if (searchType.IsInstanceOfType(item)) return true;

            var type = item.GetType();
            return Validate(type, searchType);
        }

        public static bool Validate(Type type, Type searchType)
        {
            if (type == null) return false;
            if (type.IsValueType)
                return false;
            if (type == _stringType && searchType != _stringType)
            {
                return false;
            }
            return true;
        }

        public static void FindResources<TData>(List<Object> assets, Action<Object, TData> onFoundAction, HashSet<object> excludedItems = null, Func<TData, TData> resourceAction = null) where TData : class
        {
            GUI.changed = true;
            var cache = excludedItems == null ? new HashSet<object>() : excludedItems;
            try
            {
                foreach (var asset in assets)
                {
                    FindResource<TData>(asset, onFoundAction, cache, resourceAction);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asset">source asset</param>
        /// <param name="onFoundAction"></param>
        /// <param name="cache">exclude items map filter</param>
        /// <param name="assetAction">allow change searching field value</param>
        /// <returns></returns>
        public static void FindResource<T>(Object asset, Action<Object, T> onFoundAction, HashSet<object> cache = null,
                                                Func<T, T> assetAction = null)
        {
            GUI.changed = true;
            var resourceCache = cache == null ? new HashSet<object>() : cache;
            if (asset == null) return;
            try
            {
                var seachingType = typeof(T);
                if (seachingType.IsInstanceOfType(asset))
                {
                    if (onFoundAction != null)
                        onFoundAction(asset, (T)(object)asset);
                    return;
                }
                SearchInFieldsRecursively(asset, asset, onFoundAction, resourceCache, assetAction);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static List<Type> FindAllChildrenTypes<T>()
        {
            var types = Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)));
            return types.ToList();
        }

        public static List<Type> FindAllImplementations(Type targetType)
        {
            var type = targetType;
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));
            return types.ToList();
        }

        
        
    }


}

