namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UniGame.LeoEcs.Converter.Runtime.Abstract;
    using UniGame.LeoEcs.Shared.Components;
    using UniGame.LeoEcs.Shared.Extensions;
    using UnityEngine;
    
    [Serializable]
    public class GameObjectConverter 
        : IComponentConverter,ILeoEcsMonoComponentConverter
    {
        public bool enabled = true;
        
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [Space(8)]
        [InlineProperty]
        [SerializeReference]
        private List<ILeoEcsMonoComponentConverter> converters = new List<ILeoEcsMonoComponentConverter>();
        
        public bool IsEnabled => enabled;
        
        public void Apply(EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            var haveComponent = world.HasComponent<GameObjectComponent>(entity);
            if (!haveComponent)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"Converter {GetType().Name} doesn't have {nameof(GameObjectComponent)} on entity {entity}");
#endif
                return;
            }
            
            ref var gameObjectComponent = ref world.GetComponent<GameObjectComponent>(entity);

            Apply(gameObjectComponent.GameObject, world, entity, cancellationToken);
        }

        public void Apply(GameObject target, EcsWorld world, int entity, CancellationToken cancellationToken = default)
        {
            OnApply(target, world, entity, cancellationToken);
            foreach (var componentConverter in converters)
                componentConverter.Apply(target,world,entity,cancellationToken);
        }

        protected virtual void OnApply(GameObject target, EcsWorld world, int entity,
            CancellationToken cancellationToken = default)
        {
            
        }

        public bool IsMatch(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return true;
            if (GetType().Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            
            return false;
        }
    }
}