namespace UniGame.ECS.Mono
{
    using System;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    [Serializable]
    public class EcsSystemInstance<TSystem> : IEcsSystemInstance
        where TSystem : IEcsSystem, new()
    {
        [InlineProperty]
        public TSystem ecsSystem;
        
        public UniTask Register(IContext context,EcsWorld world,EcsSystems systems)
        {
            ecsSystem = new TSystem();
            systems.Add(ecsSystem);
            return UniTask.CompletedTask;
        }
    }
}