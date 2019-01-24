using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.ObjectPool.Scripts
{
    public static class ObjectPoolExtension
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

        public static T Spawn<T>(Action<T> action = null)
            where T : class, new()
        {
            var item = ClassPool.Spawn( action);
            return item ?? new T();
        }

        public static void DespawnGameObject(this GameObject instance, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(instance);
                return;
            }
            ObjectPool.Despawn(instance);
        }

        public static void DespawnComponent(this Component data, bool destroy = false)
        {
            if (destroy)
            {
                Object.DestroyImmediate(data.gameObject);
                return;
            }
            ObjectPool.Despawn(data);
        }
        
        public static void Despawn(this object data, bool destroy = false)
        {

            if (data == null) return;

            var component = data as Component;
            if (component != null)
            {
                DespawnComponent(component, destroy);
            }
            else if (data is GameObject gameObject)
            {
                DespawnGameObject(gameObject, destroy);
            }
            else
            {
                ClassPool.Despawn(data);
            }
            
        }
    }
}
