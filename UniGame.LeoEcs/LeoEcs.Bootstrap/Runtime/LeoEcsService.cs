using UniGame.Core.Runtime;
using UniGame.UniNodes.GameFlow.Runtime;
using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;

namespace UniGame.LeoEcs.Bootstrap.Runtime
{
    using System.Collections.Generic;
    using Abstract;
    using Config;
    using Converter.Runtime;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Object = UnityEngine.Object;

    public class LeoEcsService : GameService,ILeoEcsService
    {
        private readonly ILeoEcsSystemsConfig _config;
        private readonly IEcsExecutorFactory _ecsExecutorFactory;
        private readonly IEnumerable<ISystemsPlugin> _plugins;
        private readonly bool _ownThisWorld;
        private readonly Dictionary<string, EcsSystems> _systemsMap;
        private readonly Dictionary<string, ILeoEcsExecutor> _systemsExecutors;

        private readonly IContext _context;
        private EcsWorld _world;
        private bool _isInitialized;

        public EcsWorld World => _world;

        public LeoEcsService(
            IContext context,
            EcsWorld world, 
            ILeoEcsSystemsConfig config,
            IEcsExecutorFactory ecsExecutorFactory, 
            IEnumerable<ISystemsPlugin> plugins,
            bool ownThisWorld)
        {
            _systemsMap = new Dictionary<string, EcsSystems>(8);
            _systemsExecutors = new Dictionary<string, ILeoEcsExecutor>(8);

            _context = context;
            _world = world;
            _config = config;

            _ecsExecutorFactory = ecsExecutorFactory;
            _plugins = plugins;
            _ownThisWorld = ownThisWorld;

            LifeTime.AddCleanUpAction(CleanUp);
        }

        
        public void SetDefaultWorld(EcsWorld world)
        {
            LeoEcsConvertersData.World = world;
        }
        
        public override async UniTask InitializeAsync()
        {
            await InitializeEcsService(_world);

            _isInitialized = true;

            foreach (var systems in _systemsMap.Values)
                systems.Init();
        }

        public void Execute()
        {
            if (!_isInitialized)
                return;

            foreach (var (updateType, systems) in _systemsMap)
            {
                if (!_systemsExecutors.TryGetValue(updateType, out var executor))
                {
                    executor = _ecsExecutorFactory.Create(updateType);
                    _systemsExecutors[updateType] = executor;
                }

                executor.Execute(_world);
                executor.Add(systems);
            }

            ApplyPlugins(_world);
        }

        public void Pause()
        {
            foreach (var systemsExecutor in _systemsExecutors.Values)
                systemsExecutor.Stop();
        }

        public void CleanUp()
        {
            foreach (var systems in _systemsMap.Values)
                systems.Destroy();

            foreach (var ecsExecutor in _systemsExecutors.Values)
                ecsExecutor.Dispose();

            _systemsMap.Clear();
            _systemsExecutors.Clear();

            if (_ownThisWorld)
                _world?.Destroy();
            _world = null;
        }

        private async UniTask InitializeEcsService(EcsWorld world)
        {
            foreach (var updateGroup in _config.FeatureGroups)
                await CreateEcsGroupRunner(updateGroup.updateType, world, updateGroup.features);
        }

        private void ApplyPlugins(EcsWorld world)
        {
            foreach (var systemsPlugin in _plugins)
            {
                systemsPlugin.AddTo(LifeTime);
                
                foreach (var map in _systemsMap)
                    systemsPlugin.Add(map.Value);
                systemsPlugin.Execute(world);
            }
        }

        private async UniTask CreateEcsGroupRunner(string updateType, EcsWorld world, IReadOnlyList<LeoEcsFeatureData> features)
        {
            if (!_systemsMap.TryGetValue(updateType, out var ecsSystems))
            {
                ecsSystems = new EcsSystems(world,_context);
                _systemsMap[updateType] = ecsSystems;
            }

            foreach (var feature in features)
            {
                if (!feature.IsFeatureEnabled) continue;

                if (feature is ILeoEcsInitializableFeature initializableFeature)
                    await initializableFeature.InitializeFeatureAsync(ecsSystems);

                foreach (var system in feature.EcsSystems)
                {
                    var leoEcsSystem = system;

                    //create instance of SO systems
                    if (leoEcsSystem is Object systemAsset)
                    {
                        systemAsset = Object.Instantiate(systemAsset);
                        leoEcsSystem = systemAsset as IEcsSystem;
                    }

                    if (leoEcsSystem is ILeoEcsInitializableFeature configureAsyncSystem)
                    {
                        await configureAsyncSystem.InitializeFeatureAsync(ecsSystems);
                    }

                    ecsSystems.Add(leoEcsSystem);
                }
            }
        }
    }
}