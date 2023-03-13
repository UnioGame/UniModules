namespace UniGame.LeoEcs.Converter.Runtime.Converters
{
    using System.Threading;
    using Leopotam.EcsLite;
    using Shared.Components;
    using Shared.Extensions;
    using UnityEngine;

    public sealed class ColliderConverter : MonoLeoEcsConverter
    {
        [SerializeField]
        private Collider _collider;
        
        public override void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            ref var colliderComponent = ref world.AddComponent<ColliderComponent>(entity);
            colliderComponent.Value = _collider;
        }
    }
}