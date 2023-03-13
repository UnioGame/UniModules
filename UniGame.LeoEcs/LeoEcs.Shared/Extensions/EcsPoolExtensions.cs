namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Runtime.CompilerServices;
    using Leopotam.EcsLite;

    public static class EcsPoolExtensions
    {
        public static bool TryAdd<T>(this EcsPool<T> pool, EcsPackedEntity packedEntity) where T : struct
        {
            var world = pool.GetWorld();
            if (!packedEntity.Unpack(world, out var entity) || pool.Has(entity))
                return false;

            pool.Add(entity);
            return true;
        }
        
        public static bool TryAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (pool.Has(entity)) 
                return false;
            
            pool.Add(entity);
            return true;
        }
        
        public static bool TryGet<T>(this EcsPool<T> pool, int entity, ref T component) where T : struct
        {
            if (!pool.Has(entity)) 
                return false;
            
            component = ref pool.Get(entity);
            return true;
        }
        
#if ENABLE_IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool TryRemove<T>(this EcsPool<T> pool, EcsPackedEntity packedEntity)
            where T : struct
        {
            var world = pool.GetWorld();
            if (!packedEntity.Unpack(world, out var entity))
                return false;
            
            return TryRemove<T>(pool, entity);
        }
        
#if ENABLE_IL2CPP
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public static bool TryRemove<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
                return false;
            
            pool.Del(entity);
            return true;
        }
        
        public static bool TryAdd<T>(this EcsPool<T> pool, EcsPackedEntity packedEntity, ref T component) where T : struct
        {
            var world = pool.GetWorld();
            if (!packedEntity.Unpack(world, out var entity))
                return false;

            component = ref pool.Add(entity);
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref TComponent GetOrAddComponent<TComponent>(this EcsPool<TComponent> pool, int entity)
            where TComponent : struct
        {
            ref var component = ref pool.Has(entity) 
                ? ref pool.Get(entity) 
                : ref pool.Add(entity);
            return ref component;
        }
    }
}