namespace UniGame.ECS.Mono
{
    using System;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UnityEngine;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
    [Serializable]
    public class UniGameEcsSystemInstance : IEcsSystemInstance
    {
        [SerializeReference]
#if ODIN_INSPECTOR
        [InlineProperty]
        [HideLabel]
#endif
        public IUniGameLeoEcsSystem system;

        public async UniTask Register(IContext context, EcsWorld world, EcsSystems systems)
        {
            await system.RegisterAsync(context);
            systems.Add(system);
        }
    }
}