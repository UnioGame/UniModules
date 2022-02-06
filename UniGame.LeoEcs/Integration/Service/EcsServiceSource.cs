namespace Assets.UniGame.ECS.Service
{
    using Cysharp.Threading.Tasks;
    using global::UniGame.ECS.Mono;
    using global::UniGame.ECS.Tools;
    using Leopotam.EcsLite;
    using Services;
    using UniModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using UniModules.UniGameFlow.GameFlow.Runtime.Services;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = "Game/Ecs/" + nameof(EcsServiceSource),fileName = nameof(EcsServiceSource))]
    public class EcsServiceSource : ServiceDataSourceAsset<IEcsService>
    {
        public bool registerGlobal = false;
        
        public AssetReferenceT<EcsServiceConfiguration> configuration;

        protected override async UniTask<IEcsService> CreateServiceInternalAsync(IContext context)
        {
            var config       = await configuration.LoadAssetInstanceTaskAsync<EcsServiceConfiguration>(LifeTime);
            var runnerConfig = await config.systemsConfiguration.LoadAssetInstanceTaskAsync<EcsSystemConfigurationAsset>(LifeTime);

            var ecsRunner = new EcsRunner(runnerConfig);
            var world     = new EcsWorld();
            var service   = new EcsService(ecsRunner,world,config);

            service.Execute(context,LifeTime).AttachExternalCancellation(LifeTime.TokenSource)
                .Forget();

            if (registerGlobal)
                GlobalWorldData.GameWorld = world;
            
            return service;
        }
    }
}
