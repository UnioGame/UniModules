namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Leopotam.EcsLite;
    using UniGame.ViewSystem.Runtime;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class CloseAllViewsSystem : IEcsInitSystem,IEcsRunSystem
    {
        private readonly IGameViewSystem _gameViewSystem;
        private EcsWorld _world;

        private EcsFilter _closeAllFilter;

        public CloseAllViewsSystem(IGameViewSystem gameViewSystem)
        {
            _gameViewSystem = gameViewSystem;
        }

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            _closeAllFilter = _world.Filter<CloseAllViewsRequest>().End();
        }
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in _closeAllFilter)
            {
                _gameViewSystem.CloseAll();
                break;
            }
        }
    }
}