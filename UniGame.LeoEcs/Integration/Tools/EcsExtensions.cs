namespace UniGame.ECS.Tools
{
    using Leopotam.EcsLite;

    public static class EcsExtensions
    {
        public static ref TComponent GetComponent<TComponent>(this EcsSystems ecsSystems, int entityId)
            where TComponent : struct
        {
            return ref ecsSystems.GetWorld().GetComponent<TComponent>(entityId);
        }

        public static ref TComponent GetOrCreateComponent<TComponent>(this EcsWorld world, int entityId)
            where TComponent : struct
        {
            var pool = world.GetPool<TComponent>();
            var has  = pool.Has(entityId);
            ref var component = ref has 
                ? ref pool.Get(entityId)
                : ref pool.Add(entityId);

            return ref component;
        }

        public static ref TComponent GetComponent<TComponent>(this EcsWorld world, int entityId)
            where TComponent : struct
        {
            var     pool      = world.GetPool<TComponent>();
            ref var component = ref pool.Get(entityId);
            return ref component;
        }
    }
}