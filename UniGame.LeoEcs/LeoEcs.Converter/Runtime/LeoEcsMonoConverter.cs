namespace UniGame.LeoEcs.Converter.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Abstract;
    using Cysharp.Threading.Tasks;
    using Leopotam.EcsLite;
    using Sirenix.OdinInspector;
    using UnityEngine;

    public class LeoEcsMonoConverter : MonoBehaviour, ILeoEcsMonoConverter
    {
        #region inspector data

        [SerializeField]
        private bool _destroyEntityOnDisable = true;
        [SerializeField]
        private bool _createEntityOnEnabled = true;
        [SerializeField]
        private bool _createEntityOnStart = false;
        [SerializeField]
        private bool _destroyEntityOnDestroy = true;

        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        [Space(8)]
        [InlineProperty]
        [SerializeReference]
        public List<ILeoEcsMonoComponentConverter> _serializableConverters = new List<ILeoEcsMonoComponentConverter>();

        [Space(8)]
        [Searchable(FilterOptions = SearchFilterOptions.ISearchFilterableInterface)]
        public List<LeoEcsConverterAsset> assetConverters = new List<LeoEcsConverterAsset>();

        [Space] 
        [ReadOnly] 
        [BoxGroup("runtime info")] 
        [ShowIf(nameof(IsRuntime))] 
        [SerializeField]
        private int _ecsEntityId;
        
        private EcsPackedEntity _entityId;

        #endregion

        #region private data

        private List<ILeoEcsComponentConverter> _converters = new List<ILeoEcsComponentConverter>();

        private bool _entityCreated;

        private EcsWorld _world;

        private CancellationTokenSource _tokenSource;

        #endregion

        public bool IsRuntime => Application.isPlaying;

        public bool IsPlayingAndReady => IsRuntime && _entityCreated;

        public EcsPackedEntity EntityId => _entityId;

        #region public methods

        
        
        public async UniTask<EcsPackedEntity> Convert()
        {
            var world = await gameObject.WaitWorldReady(_tokenSource.Token);
            return Convert(world);
        }

        public EcsPackedEntity Convert(EcsWorld world)
        {
            if (_entityCreated || world.IsAlive() == false) 
                return _entityId;

            _world = world;
            _entityCreated = true;
            _ecsEntityId = gameObject.CreateEcsEntityFromGameObject(world,
                    _converters, false, 
                    _tokenSource.Token);

            _entityId = world.PackEntity(_ecsEntityId);
            
            world.ApplyEcsComponents(_ecsEntityId,assetConverters,_tokenSource.Token);
            
            return _entityId;
        }
        
        #endregion

        #region unity methods

        // Start is called before the first frame update
        private void Start()
        {
            if (_createEntityOnStart)
                CreateEntity();
        }

        private void OnDestroy()
        {
            if (_destroyEntityOnDestroy)
                DestroyEntity();
            
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
        }

        private void Awake()
        {
            _tokenSource ??= new CancellationTokenSource();
            //get all converters
            _converters ??= new List<ILeoEcsComponentConverter>();
            _converters.AddRange(_serializableConverters);
            _converters.AddRange(GetComponents<ILeoEcsComponentConverter>());
        }

        private void OnDisable()
        {
            if (_destroyEntityOnDisable)
                DestroyEntity();
        }

        private void OnEnable()
        {
            if (_createEntityOnEnabled)
                CreateEntity();
        }

        #endregion

        #region private methods

        private void CreateEntity()
        {
            Convert().Forget();
        }

        [ShowIf(nameof(IsPlayingAndReady))]
        [Button("Destroy")]
        private void DestroyObject()
        {
            DestroyEntity();
            Destroy(gameObject);
        }
        
        private void DestroyEntity()
        {
            if (!_entityCreated) return;

            //notify converters about destroy
            foreach (var converter in _converters)
            {
                if(converter is IConverterEntityDestroyHandler destroyHandler)
                    destroyHandler.OnEntityDestroy(_world,_ecsEntityId);
            }
            
            //notify converters about destroy
            foreach (var converter in assetConverters)
            {
                if(converter is IConverterEntityDestroyHandler destroyHandler)
                    destroyHandler.OnEntityDestroy(_world,_ecsEntityId);
            }
            
            _entityCreated = false;
            LeoEcsTool.DestroyEntity(_entityId, _world);
        }

        #endregion
        
#if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            foreach (var converter in _serializableConverters)
            {
                if(converter is ILeoEcsGizmosDrawer gizmosDrawer)
                    gizmosDrawer.DrawGizmos(gameObject);
            }
            foreach (var converter in assetConverters)
            {
                if(converter is ILeoEcsGizmosDrawer gizmosDrawer)
                    gizmosDrawer.DrawGizmos(gameObject);
            }
        }

#endif
    }
}