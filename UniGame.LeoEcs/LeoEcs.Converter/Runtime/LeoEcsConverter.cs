namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Threading;
    using Abstract;
    using Leopotam.EcsLite;
    using Shared.Components;
    using Shared.Extensions;
    using Sirenix.OdinInspector;
    using UnityEngine;

    [Serializable]
    public abstract class LeoEcsConverter : ILeoEcsMonoComponentConverter,IComponentConverter
    {
        [SerializeField]
        private bool _isEnabled = true;

        public virtual bool IsEnabled => _isEnabled;
        
        public abstract void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default);
        
        public void Apply(EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            if (!world.HasComponent<GameObjectComponent>(entity)) return;

            ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);

            if (gameObjectComponent.GameObject == null) return;
            
            Apply(gameObjectComponent.GameObject,world,entity,cancellationToken);
        }
        
        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;

            if (this.GetType().Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }


    }
}