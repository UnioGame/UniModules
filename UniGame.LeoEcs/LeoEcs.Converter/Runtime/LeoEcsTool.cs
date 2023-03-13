using UniGame.LeoEcs.Shared.Components;
using UniGame.LeoEcs.Shared.Extensions;

namespace UniGame.LeoEcs.Converter.Runtime
{
    using System.Collections.Generic;
    using System.Threading;
    using Abstract;
    using Converters;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UnityEngine;

    public static class LeoEcsTool
    {
        /// <summary>
        /// base common converters for gameobjects
        /// </summary>
        public static List<ILeoEcsMonoComponentConverter> DefaultGameObjectConverters = new List<ILeoEcsMonoComponentConverter>()
        {
            new BaseGameObjectComponentConverter()
        };

        public static void ApplyEcsComponents(this GameObject target, 
            EcsWorld world, 
            int entityId, 
            IEnumerable<ILeoEcsComponentConverter> converterTasks,
            CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            if (target == null)
                Debug.LogError($"ECS CONVERTER: GameObject {target} is NULL | ENTITY {entityId}");
#endif
            foreach (var converter in converterTasks)
            {
#if UNITY_EDITOR
                if (converter == null)
                    Debug.LogError($"ECS CONVERTER: Converter == null FOR GameObject {target} is NULL | ENTITY {entityId}",target);
#endif
                if(converter is ILeoEcsConverterStatus converterStatus && !converterStatus.IsEnabled)
                    continue;
                converter.Apply(target, world, entityId, cancellationToken);
            }
        }
        
        public static void ApplyEcsComponents(
            this EcsWorld world, 
            int entityId, 
            IEnumerable<IComponentConverter> converterTasks,
            CancellationToken cancellationToken = default)
        {
#if UNITY_EDITOR
            GameObject gameObject = default;
            if (world.HasComponent<GameObjectComponent>(entityId))
            {
                ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entityId);
                gameObject = gameObjectComponent.GameObject;
            }
#endif
            foreach (var converter in converterTasks)
            {
                var targetConverter = converter;
#if UNITY_EDITOR
                if (targetConverter == null)
                {
                    Debug.LogError($"ECS CONVERTER: Converter == null FOR GameObject {gameObject?.name} is NULL | ENTITY {entityId}",gameObject);
                }
#endif
                if (targetConverter is ScriptableObject scriptableConverter)
                    targetConverter = Object.Instantiate(scriptableConverter) as IComponentConverter;
                
                if(targetConverter is ILeoEcsConverterStatus converterStatus && !converterStatus.IsEnabled)
                    continue;
                
                targetConverter.Apply(world, entityId, cancellationToken);
            }
        }

        public static int Convert(this GameObject target,EcsWorld world)
        {
            if (target == null)
                return -1;
            
            var converter = target.GetComponent<ILeoEcsMonoConverter>();
            var packedEntity = converter.Convert(world);
            if (packedEntity.Unpack(world, out var resultEntity))
                return resultEntity;
            return -1;
        }
        
        public static async UniTask<int> CreateEcsEntityFromGameObject(this GameObject target, IEnumerable<ILeoEcsMonoComponentConverter> converterTasks, bool spawnInstance,
            CancellationToken cancellationToken = default)
        {
            var world = await WaitWorldReady(cancellationToken);
            var entity = CreateEcsEntityFromGameObject(target, world, converterTasks, spawnInstance, cancellationToken);
            return entity;
        }

        public static int CreateEcsEntityFromGameObject(this GameObject target, 
            EcsWorld world, 
            IEnumerable<ILeoEcsComponentConverter> converterTasks, 
            bool spawnInstance,
            CancellationToken cancellationToken = default)
        {
            target = spawnInstance ? Object.Instantiate(target) : target;
            var entity = world.NewEntity();

            ApplyEcsComponents(target, world, entity, DefaultGameObjectConverters, cancellationToken);
            ApplyEcsComponents(target, world, entity, converterTasks, cancellationToken);

            return entity;
        }

        public static async UniTask DestroyEntity(int entityId, CancellationToken cancellationToken = default)
        {
            var world = await WaitWorldReady(cancellationToken);
            DestroyEntity(entityId, world);
        }
        
        public static async UniTask DestroyEntity(EcsPackedEntity entityId, CancellationToken cancellationToken = default)
        {
            var world = await WaitWorldReady(cancellationToken);
            DestroyEntity(entityId, world);
        }
        
        public static void DestroyEntity(EcsPackedEntity entityId, EcsWorld world)
        {
            if(entityId.Unpack(world,out var entity))
                DestroyEntity(entity, world);
        }

        public static void DestroyEntity(int entityId, EcsWorld world)
        {
            if (!world.IsAlive())
                return;
            
            var packed = world.PackEntity(entityId);
            if(packed.Unpack(world,out var aliveEntity))
                world.DelEntity(aliveEntity);
        }

        public static async UniTask<EcsWorld> WaitWorldReady(this GameObject target,CancellationToken cancellationToken = default)
        {
            return await WaitWorldReady(cancellationToken);
        }

        public static async UniTask<EcsWorld> WaitWorldReady(CancellationToken cancellationToken = default)
        {
            await UniTask.WaitUntil(() => LeoEcsConvertersData.World != null && 
                                          LeoEcsConvertersData.World.IsAlive(), cancellationToken: cancellationToken);
            return LeoEcsConvertersData.World;
        }
    }
}