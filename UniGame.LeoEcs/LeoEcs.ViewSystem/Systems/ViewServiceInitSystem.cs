namespace UniGame.LeoEcs.ViewSystem.Systems
{
    using System;
    using Components;
    using Context.Runtime.Extension;
    using Core.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Shared.Extensions;
    using Shared.Systems;
    using UniCore.Runtime.ProfilerTools;
    using UniGame.ViewSystem.Runtime;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [Serializable]
    public class ViewServiceInitSystem : IEcsInitSystem
    {
        private readonly IGameViewSystem _gameViewSystem;
        private EcsWorld _world;

        public ViewServiceInitSystem(IGameViewSystem gameViewSystem)
        {
            _gameViewSystem = gameViewSystem;
        }

        public void Init(EcsSystems systems)
        {
            _world = systems.GetWorld();
            var entity = _world.NewEntity();
            ref var component = ref _world.AddComponent<ViewServiceComponent>(entity);
            component.ViewSystem = _gameViewSystem;
            
            GameLog.Log($"{nameof(ViewServiceComponent)} Created",Color.green);
        }
    }
}
