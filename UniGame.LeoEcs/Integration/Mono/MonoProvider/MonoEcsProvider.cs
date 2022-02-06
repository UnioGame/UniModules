namespace UniGame.ECS.Mono.MonoProvider
{
    using System;
    using System.Collections.Generic;
    using Abstract;
    using Components;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Tools;
    using UniModules.UniCore.Runtime.Attributes;
    using UniModules.UniGame.Core.Runtime.DataFlow.Extensions;
    using UniModules.UniGame.Core.Runtime.DataFlow.Interfaces;
    using UnityEngine;

    public class MonoEcsProvider : MonoBehaviour, IMonoEcsProvider
    {
        public static Type baseComponentType = typeof(Component);

        public bool registerAtGlobalStart = false;
        
        [ReadOnlyValue]
        public int entityId;

        [SerializeReference]
        public List<IEcsComponentSource> componentSources = new List<IEcsComponentSource>();

        [SerializeField]
        public Component[] components;

        private EcsWorld  _world;
        private ILifeTime _lifeTime;

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void RegisterAtWorld()
        {
            _lifeTime = this.GetAssetLifeTime();
            InitializeOnStartAsync(_lifeTime).Forget();
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Refresh()
        {
            if (_world == null) return;
            _world.DelEntity(entityId);
            RegisterAtWorld();
        }
        
        public int CreateWorldEntity(EcsWorld world)
        {
            _world     =   world;
            entityId   =   _world.NewEntity();
            
            components ??= GetComponents(baseComponentType);

            ref var behaviourComponents = ref _world.GetOrCreateComponent<BehaviourComponents>(entityId);
            behaviourComponents.components = components;

            ref var prefabTransformComponent = ref _world.GetOrCreateComponent<TransformPositionComponent>(entityId);
            prefabTransformComponent.transform = transform;
            
            foreach (var component in components)
            {
                if(component is IEcsComponentSource componentSource)
                    componentSource.CreateComponent(_world,entityId);
            }

            foreach (var source in componentSources)
                source.CreateComponent(_world,entityId);

            return entityId;
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            if(registerAtGlobalStart)
                RegisterAtWorld();
        }

        private async UniTask InitializeOnStartAsync(ILifeTime lifeTime)
        {
            if (_world == null)
            {
                await UniTask.WaitWhile(() => _world == null && GlobalWorldData.GameWorld == null).AttachExternalCancellation(lifeTime.TokenSource);
            }
            
            var world = _world ?? GlobalWorldData.GameWorld;
            CreateWorldEntity(world);
        }
        
        private void OnDestroy()
        {
            if (_world == null || !_world.IsAlive()) return;
            _world.DelEntity(entityId);
        }
    }
}
