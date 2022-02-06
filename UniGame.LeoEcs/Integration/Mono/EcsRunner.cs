namespace UniGame.ECS.Mono
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using UniModules.UniCore.Runtime.DataFlow;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;
    using Object = UnityEngine.Object;

    [Serializable]
    public class EcsRunner : IEcsRunner
    {
        private readonly EcsSystemConfigurationAsset _configurationAsset;

        #region private data

        private bool               _isRunning = false;
        private LifeTimeDefinition _lifeTime;
        
#endregion

        public EcsRunner(EcsSystemConfigurationAsset configurationAsset)
        {
            _configurationAsset = configurationAsset;
        }

        public ILifeTime LifeTime => _lifeTime;

        public async UniTask Execute(IContext context,EcsWorld world)
        {
            if (_isRunning) return;
            
            _isRunning = true;
            _lifeTime  = new LifeTimeDefinition();
            _lifeTime.AddCleanUpAction(() => _isRunning = false);
            
            var ecsAsset = Object.Instantiate(_configurationAsset);
            var configuration = ecsAsset.configuration;

            foreach (var ecsSystemsGroup in configuration.systems)
            {
                var systems = CreateSystemsGroup(world);

                await CreateSystems(ecsSystemsGroup.ecsSystems,context, world, systems);
                
                systems.Init();
                _lifeTime.AddCleanUpAction(() => systems.Destroy());
                
                UpdateEcsGroup(systems,ecsSystemsGroup.updateType)
                    .AttachExternalCancellation(_lifeTime.TokenSource)
                    .SuppressCancellationThrow()
                    .Forget();
            }
            
        }

        public void Dispose() => _lifeTime?.Terminate();

        protected virtual EcsSystems CreateSystemsGroup(EcsWorld world) => new EcsSystems(world);

        protected virtual async UniTask CreateSystems(IEnumerable<IEcsSystemInstance> systemInstances,IContext context, EcsWorld world,EcsSystems  ecsSystemsGroup)
        {
            foreach (var systemInstance in systemInstances)
                await systemInstance.Register(context,world, ecsSystemsGroup);
        }

        private async UniTask UpdateEcsGroup(EcsSystems systemGroup,PlayerLoopTiming updateType)
        {
            do
            {
                await UniTask.Yield(updateType);
                systemGroup.Run();
            } while (!_lifeTime.IsTerminated);
        }
        
    }
}
