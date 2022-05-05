namespace Assets.UniGame.ECS.Service.Services
{
    using Cysharp.Threading.Tasks;
    using global::UniGame.ECS.Mono.Abstract;
    using global::UniGame.UniNodes.GameFlow.Runtime;
    using Leopotam.EcsLite;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UniModules.UniGame.Core.Runtime.Interfaces;

    public class EcsService : GameService, IEcsService
    {
        private readonly IEcsRunner              _ecsRunner;
        private readonly EcsWorld                _world;
        private readonly EcsServiceConfiguration _configuration;

        public EcsService(IEcsRunner ecsRunner, EcsWorld world, EcsServiceConfiguration configuration)
        {
            _ecsRunner          = ecsRunner;
            _world              = world;
            _configuration = configuration;
            
            Complete();
        }

        public async UniTask Execute(IContext context, ILifeTime lifeTime)
        {
            await _ecsRunner.Execute(context, _world).AttachExternalCancellation(lifeTime.TokenSource);
        }
    
    }
}
