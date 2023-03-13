namespace UniGame.LeoEcs.Shared.Extensions
{
    using System.Collections.Generic;
    using Abstract;
    using Core.Runtime;
    using Leopotam.EcsLite;
    using UnityEngine;

    public static class EntityExtensions
    {
        private static List<IInitializeWithEntityData> InitializableComponents = new List<IInitializeWithEntityData>();

        public static ILifeTime DestroyEntityWith(this ILifeTime lifeTime, int entity, EcsWorld world)
        {
            if (entity < 0 || world == null || world.IsAlive() == false) return lifeTime;
            
            lifeTime.AddCleanUpAction(() =>
            {
                if (!world.IsAlive()) return;
                var packedEntity = world.PackEntity(entity);
                if (!packedEntity.Unpack(world, out var unpacked))
                    return;
                
                world.DelEntity(entity);
            });
            
            return lifeTime;
        }

        public static Object InitializeWithEcsData(this Object target, EcsWorld world, int entity)
        {
            switch (target)
            {
                case IInitializeWithEntityData initializable:
                    initializable.InitializeEcsData(world, entity);
                    break;
                case GameObject gameObject:
                {
                    InitializableComponents.Clear();
                    gameObject.GetComponents(InitializableComponents);

                    foreach (var initializableComponent in InitializableComponents)
                        initializableComponent.InitializeEcsData(world, entity);

                    InitializableComponents.Clear();
                    
                    
                    
                    break;
                }
            }

            return target;
        }
    }
}