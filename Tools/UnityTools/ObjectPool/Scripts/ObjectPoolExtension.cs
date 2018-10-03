using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Tools.Utils
{
    public static class ObjectPoolExtension
    {

        public static void Despawn<TData>(this ICollection<TData> data)
        {
            data.Clear();
            ClassPool<ICollection<TData>>.Despawn(data);
        }

        public static void DespawnRecursive<TData>(this List<TData> data)
            where TData : class 
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] is IPoolable)
                    data[i].Despawn();
            }
            data.Clear();
            ClassPool<List<TData>>.Despawn(data);
        }

        public static void Despawn<TData>(this List<TData> data)
        {
            data.Clear();
            ClassPool<List<TData>>.Despawn(data);
        }

        public static void Despawn<TKey, TData>(this Dictionary<TKey, TData> data)
        {
            data.Clear();
            ClassPool<Dictionary<TKey, TData>>.Despawn(data);
        }

        public static TComponent Spawn<TComponent>(this GameObject prototype, int preloadsCount = 0)
            where TComponent : Component
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<TComponent>(prototype);
            return pawn;
        }

        public static TComponent Spawn<TComponent>(this TComponent prototype, Vector3 position,
            Quaternion rotation, Transform parent = null, bool stayWorldPosition = false)
            where TComponent : Component
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<TComponent>(prototype, position, rotation, parent, stayWorldPosition);
            return pawn;
        }


        public static TComponent Spawn<TComponent>(this TComponent prototype, Transform parent = null, bool stayWorldPosition = false)
            where TComponent : Component
        {
            if (!prototype) return null;
            var pawn = ObjectPool.Spawn<TComponent>(prototype, Vector3.zero, Quaternion.identity,
                parent, stayWorldPosition);
            return pawn;
        }

        public static T Spawn<T>(Predicate<T> predicate = null, Action<T> action = null)
            where T : class, new()
        {
            var item = ClassPool<T>.Spawn(predicate, action);
            return item ?? new T();
        }

        public static void Despawn(this GameObject instance, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(instance);
                return;
            }
            ObjectPool.Despawn(instance);
        }

        public static void Despawn(this Component data, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(data.gameObject);
                return;
            }
            ObjectPool.Despawn(data);
        }

        public static void Despawn<T>(this T data, bool destroy = false)
            where T : class
        {

            if (data == null) return;

            var component = data as Component;
            if (component != null)
                Despawn(component, destroy);
            else
            {
                ClassPool<T>.Despawn(data);
            }
        }
    }
}
