namespace UniGame.SceneEditorOnlyAssets.Runtime
{
    using System;
    using Addressables.Reactive;
    using Cysharp.Threading.Tasks;
    using UniCore.Runtime.ProfilerTools;
    using UniGreenModules.UniCore.Runtime.DataFlow;
    using UniGreenModules.UniCore.Runtime.Rx.Extensions;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Extensions;
    using UniRx;
    
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class SceneAsset : MonoBehaviour, ISceneEditorAsset
    {
        #region inspector

        [SerializeField]
        private AssetReferenceGameObject _target;

        [SerializeField]
        private Transform _parent;

        [SerializeField]
        private GameObject _asset;

        [SerializeField]
        public bool _createInstanceAtPlayMode;

        [SerializeField]
        public float _spawnDelay = 0.1f;
        
        #endregion

        private LifeTimeDefinition _lifeTime = new LifeTimeDefinition();

        #region public properties

        public GameObject Target
        {
            get
            {
#if  UNITY_EDITOR
                return _target.editorAsset as GameObject;
#endif
                return null;
            }
        }

        public Transform Parent => _parent ?? transform;

        public GameObject Asset => _asset;

        public bool IsOpen => _asset != null;

        #endregion

        public virtual void Save()
        {

        }

        public void Close()
        {
            if (!_asset)
            {
                return;
            }
            Save();
            OnClose(_asset);
            Reset();
        }

        public void Reset()
        {
            if (_asset) {
                DestroyImmediate(_asset, true);
                _asset = null;
            }
            _lifeTime?.Release();
        }

        public async UniTask<GameObject> OpenRuntime()
        {
            if (_asset)
                return _asset;
            
            await UniTask.Delay(TimeSpan.FromSeconds(_spawnDelay));
            
            var asset = await _target.
                LoadAssetTaskAsync(_lifeTime);

            if (_asset)
                return asset;
            
            return GameObject.Instantiate(asset, Parent);
            
        }

        public void Open()
        {

#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            var target = _target.editorAsset;
            if (!target || _asset)
                return;
            _asset = UnityEditor.PrefabUtility.InstantiatePrefab(target, Parent) as GameObject;
            OnOpen(_asset);
#endif

        }

        private void Awake()
        {
            _lifeTime = new LifeTimeDefinition();
        }

        protected async void Start()
        {
            _lifeTime?.Release();
            
            if (!Application.isPlaying || !_createInstanceAtPlayMode) {
                return;
            }

            var asset = await OpenRuntime();
            
            OnStart(asset);

        }

        protected void OnDestroy() => _lifeTime?.Terminate();

        protected virtual void OnStart(GameObject asset)
        {

        }

        protected virtual void OnOpen(GameObject asset)
        {

        }

        protected virtual void OnClose(GameObject asset)
        {

        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void CreateAsset() => OpenRuntime();
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        private void UnloadAsset() => Reset();

    }
}
