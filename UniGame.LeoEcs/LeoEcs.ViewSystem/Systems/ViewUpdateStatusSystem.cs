using UniGame.LeoEcs.ViewSystem.Components;

namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Leopotam.EcsLite;
    
    [Serializable]
    public class ViewUpdateStatusSystem : IEcsInitSystem,IEcsRunSystem
    {
        private EcsFilter _viewFilter;
        private EcsWorld _world;
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _viewFilter = _world
                .Filter<ViewComponent>()
                .Inc<ViewStatusComponent>()
                .End();
        }

        public void Run(EcsSystems systems)
        {
            var viewPool = _world.GetPool<ViewComponent>();
            var statusPool = _world.GetPool<ViewStatusComponent>();
            
            foreach (var entity in _viewFilter)
            {
                ref var viewComponent = ref viewPool.Get(entity);
                ref var viewStatusComponent = ref statusPool.Get(entity);

                var view = viewComponent.View;
                viewStatusComponent.Status = view.Status.Value;
            }
        }
    }
}
