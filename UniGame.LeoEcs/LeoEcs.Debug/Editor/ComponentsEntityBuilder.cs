namespace UniGame.LeoEcs.Debug.Editor
{
    using System;
    using System.Buffers;
    using System.Collections.Generic;
    using Leopotam.EcsLite;
    using Runtime.ObjectPool;
    using Runtime.ObjectPool.Extensions;

    [Serializable]
    public class ComponentsEntityBuilder : IEntityEditorViewBuilder
    {
        private EcsWorld _world;

        public void Initialize(EcsWorld world)
        {
            _world = world;
        }

        public void Execute(List<EntityEditorView> views)
        {
            foreach (var view in views)
            {
                ref var packedEntity = ref view.packedEntity;
                if (!packedEntity.Unpack(_world, out var entity))
                {
                    view.isDead = true;
                    continue;
                }

                var componentsCount = _world.GetComponentsCount(entity);
                var components = ArrayPool<object>.Shared.Rent(componentsCount);
                _world.GetComponents(view.id, ref components);

                foreach (var component in components)
                {
                    if(component == null) continue;
                    
                    var componentView = ClassPool.Spawn<ComponentEditorView>();
                    componentView.entity = view.id;
                    componentView.value = component;
                    
                    view.components.Add(componentView);
                }
                
                components.Despawn();
            }
        }
    }
}