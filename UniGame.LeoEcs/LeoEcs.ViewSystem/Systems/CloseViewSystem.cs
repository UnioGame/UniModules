namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class CloseViewSystem : IEcsInitSystem,IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter _closeFilter;
        private EcsPool<ViewComponent> _viewComponent;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _closeFilter = _world
                .Filter<CloseViewRequest>()
                .Inc<ViewComponent>()
                .End();

            _viewComponent = _world.GetPool<ViewComponent>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _closeFilter)
            {
                ref var viewComponent = ref _viewComponent.Get(entity);
                viewComponent.View.Close();
                break;
            }
        }
    }
}