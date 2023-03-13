namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Leopotam.EcsLite;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public static class EcsWorldExtensions
    {
        
        public static ref TComponent AddComponentToEntity<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            ref var component = ref pool.Add(entity);
            
            return ref component;
        }

        public static bool AnyHas<TPool>(this EcsWorld world, List<int> entities)
            where TPool : struct
        {
            var pool = world.GetPool<TPool>();
            foreach (var entity in entities)
            {
                if (pool.Has(entity))
                    return true;
            }

            return false;
        }

        public static ref TComponent GetComponent<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            return ref pool.Get(entity);
        }

        public static IEnumerable<EcsPackedEntity> PackAll(this EcsWorld world, IEnumerable<int> entities)
        {
            foreach (var entity in entities)
            {
                yield return world.PackEntity(entity);
            }
        }

        public static bool UnpackAll(this EcsWorld world,List<int> result, List<EcsPackedEntity> packedEntities)
        {
            var unpackResult = true;
            foreach (var ecsPackedEntity in packedEntities)
            {
                if (!ecsPackedEntity.Unpack(world, out var entity))
                {
                    unpackResult = false;
                    continue;
                }
                result.Add(entity);
            }

            return unpackResult;
        }

#if ENABLE_IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool TryRemoveComponent<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            if (!pool.Has(entity))
                return false;

            pool.Del(entity);
            return true;
        }

#if ENABLE_IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool HasComponent<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            return pool.Has(entity);
        }
        
        public static ref TComponent AddComponent<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            ref var component = ref pool.Add(entity);
            
            return ref component;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetOrAddComponent<TComponent>(this EcsWorld world, int entity)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            ref var component = ref pool.Has(entity) ? ref pool.Get(entity) : ref pool.Add(entity);
            return ref component;
        }

        public static void FilterByComponent<T>(this EcsWorld world, IEnumerable<int> filter, List<int> result) where T : struct
        {
            var pool = world.GetPool<T>();
            foreach (var entity in filter)
            {
                if (pool.Has(entity))
                    result.Add(entity);
            }
        }

        public static void FilterByComponent<T, T2>(this EcsWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            foreach (var entity in filter)
            {
                if (pool.Has(entity) && pool2.Has(entity))
                    result.Add(entity);
            }
        }

        public static void FilterByComponent<T, T2, T3>(this EcsWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
            where T3 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();
            foreach (var entity in filter)
            {
                if (pool.Has(entity) && pool2.Has(entity) && pool3.Has(entity))
                    result.Add(entity);
            }
        }

        public static void FilterByComponent<T, T2, T3, T4>(this EcsWorld world, IEnumerable<int> filter, List<int> result)
            where T : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            var pool = world.GetPool<T>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();
            var pool4 = world.GetPool<T4>();

            foreach (var entity in filter)
            {
                if (pool.Has(entity) && pool2.Has(entity) && pool3.Has(entity) && pool4.Has(entity))
                    result.Add(entity);
            }
        }
    }
}
