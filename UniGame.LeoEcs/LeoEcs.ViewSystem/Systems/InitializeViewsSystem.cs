namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Behavriour;
    using Components;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using UniGame.ViewSystem.Runtime;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class InitializeViewsSystem : IEcsInitSystem,IEcsRunSystem
    {
        private readonly IEcsViewTools _viewTools;
        
        private EcsWorld _world;
        private EcsFilter _filter;
        
        private EcsPool<ViewInitializedComponent> _viewInitializedPool;
        private EcsPool<ViewComponent> _viewComponentPool;
        
        public InitializeViewsSystem(IEcsViewTools viewTools)
        {
            _viewTools = viewTools;
        }
        
        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            
            _filter = _world
                .Filter<ViewComponent>()
                .Exc<ViewInitializedComponent>()
                .End();

            _viewInitializedPool = _world.GetPool<ViewInitializedComponent>();
            _viewComponentPool = _world.GetPool<ViewComponent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _viewInitializedPool.Add(entity);
                
                ref var viewComponent = ref _viewComponentPool.Get(entity);
                var packedEntity = _world.PackEntity(entity);
                var view = viewComponent.View;
                var viewType = viewComponent.Type;

                if (view.IsInitialized.Value) continue;
                
                _viewTools.AddModelComponentAsync(_world, packedEntity, view, viewType)
                    .AttachExternalCancellation(_viewTools.LifeTime.TokenSource)
                    .Forget();
            }
        }

    }
}