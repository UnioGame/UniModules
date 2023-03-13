using UniGame.LeoEcs.Shared.Systems;

namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Collections.Generic;
    using Leopotam.EcsLite;

    public static class LeoEcsExtensions
    {
        
        public static void FireOn<TFilter,TComponent>(this EcsSystems systems)
            where TFilter : struct
            where TComponent : struct
        {
            systems.Add(new FireOnSystem<TFilter,TComponent>());
        }
        
        public static void FireOn<TFilter1,TFilter2,TComponent>(this EcsSystems systems)
            where TFilter1 : struct
            where TFilter2 : struct
            where TComponent : struct
        {
            systems.Add(new FireOnSystem<TFilter1,TFilter2,TComponent>());
        }
        
        public static void FireOn<TFilter1,TFilter2,TFilter3,TComponent>(this EcsSystems systems)
            where TFilter1 : struct
            where TFilter2 : struct
            where TFilter3 : struct
            where TComponent : struct
        {
            systems.Add(new FireOnSystem<TFilter1,TFilter2,TFilter3,TComponent>());
        }
        
        public static void DelEventHere<TRequest>(this EcsSystems systems, int cyclesAmount = 1)
        {
            systems.Add(new UpdateCounterRequestSystem<TRequest>(cyclesAmount));
        }
        
        public static void DelRequestHere<TRequest>(this EcsSystems systems, int cyclesAmount = 0)
        {
            systems.Add(new UpdateCounterRequestSystem<TRequest>(cyclesAmount));
        }

        public static bool EntityHasAll<T1, T2>(this EcsWorld world,int entity)
            where T1 : struct
            where T2 : struct
        {
            var pool1 = world.GetPool<T1>();
            var pool2 = world.GetPool<T2>();

            return pool1.Has(entity) && pool2.Has(entity);
        }
        
        public static bool EntityHasAll<T1, T2,T3>(this EcsWorld world,int entity)
            where T1 : struct
            where T2 : struct
            where T3 : struct
        {
            var pool1 = world.GetPool<T1>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();

            return pool1.Has(entity) && pool2.Has(entity) && pool3.Has(entity);
        }
        
        public static bool EntityHasAll<T1, T2,T3,T4>(this EcsWorld world,int entity)
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
        {
            var pool1 = world.GetPool<T1>();
            var pool2 = world.GetPool<T2>();
            var pool3 = world.GetPool<T3>();
            var pool4 = world.GetPool<T4>();

            return pool1.Has(entity) && pool2.Has(entity) && pool3.Has(entity) && pool4.Has(entity);
        }
        
        public static EcsFilter GetFilter<TComponent>(this EcsSystems ecsSystems)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            var filter = world.Filter<TComponent>().End();

            return filter;
        }

        public static EcsPool<TComponent> GetPool<TComponent>(this EcsSystems ecsSystems)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            var pool = world.GetPool<TComponent>();

            return pool;
        }

        public static bool TryRemoveComponent<TComponent>(this EcsSystems systems, int entity)
            where TComponent : struct
        {
            var world = systems.GetWorld();
            return world.TryRemoveComponent<TComponent>(entity);
        }

        public static ref TComponent GetComponent<TComponent>(this EcsSystems ecsSystems, int entity)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            var pool = world.GetPool<TComponent>();

            return ref pool.Get(entity);
        }

        public static ref TComponent AddComponent<TComponent>(this EcsSystems ecsSystems, int entity)
            where TComponent : struct
        {
            var world = ecsSystems.GetWorld();
            return ref world.AddComponent<TComponent>(entity);
        }
        
        public static EcsSystems Add(this EcsSystems ecsSystems, IEnumerable<IEcsSystem> systems)
        {
            foreach (var system in systems)
            {
                ecsSystems.Add(system);
            }

            return ecsSystems;
        }
    }
}
