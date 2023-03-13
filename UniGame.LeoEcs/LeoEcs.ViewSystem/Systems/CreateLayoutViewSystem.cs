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
    public class CreateLayoutViewSystem : IEcsRunSystem,IEcsInitSystem
    {
        private EcsFilter _createFilter;
        private EcsWorld _world;

        private EcsPool<CreateLayoutViewRequest> _requestLayoutPool;
        private EcsPool<CreateViewRequest> _requestPool;

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _createFilter = _world.Filter<CreateLayoutViewRequest>().End();
            
            _requestLayoutPool = _world.GetPool<CreateLayoutViewRequest>();
            _requestPool = _world.GetPool<CreateViewRequest>();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _createFilter)
            {
                ref var requestLayoutComponent = ref _requestLayoutPool.Get(entity);
                ref var requestComponent = ref _requestPool.Add(entity);

                requestComponent.Parent = null;
                requestComponent.Tag = string.Empty;
                requestComponent.ViewName = string.Empty;
                requestComponent.Type = requestLayoutComponent.Type;
                requestComponent.LayoutType = requestLayoutComponent.LayoutType;
                requestComponent.StayWorld = false;

            }
        }

    }
}