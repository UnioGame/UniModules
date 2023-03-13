using UniGame.LeoEcs.Shared.Extensions;

namespace UniGame.LeoEcs.Converter.Runtime.Converters
{
    using System;
    using System.Threading;
    using Leopotam.EcsLite;
    using Shared.Components;
    using UnityEngine;

    [Serializable]
    public class BaseGameObjectComponentConverter : LeoEcsConverter
    {
        public sealed override void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            ref var transformComponent = ref world.GetOrAddComponent<TransformComponent>(entity);
            ref var gameObjectComponent = ref world.GetOrAddComponent<GameObjectComponent>(entity);

            transformComponent.Value = target.transform;
            gameObjectComponent.GameObject = target;
        }
    }
}