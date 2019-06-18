namespace UniGreenModules.UniCore.Runtime.ObjectPool
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using ProfilerTools;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class ObjectPoolExtension
    {

        public static TComponent Spawn<TComponent>(this GameObject prototype)
        {
            if (!prototype) return default(TComponent);
            
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

            GameProfiler.BeginSample("PoolExtension_Despawn");
            
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
            
            GameProfiler.EndSample();
            
        }
    }
}
