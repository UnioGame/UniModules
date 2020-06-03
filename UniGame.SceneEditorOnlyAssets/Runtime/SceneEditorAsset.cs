namespace UniGame.SceneEditorOnlyAssets.Runtime
{
    using System;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class SceneEditorAsset : MonoBehaviour, ISceneEditorAsset
    {
        #region inspector
        
        [SerializeField]
        private AssetReferenceGameObject _target;

        [SerializeField]
        private Transform _parent;
        
        [SerializeField]
        private GameObject _asset;

        #endregion
        
        #region public properties

        public GameObject Target {
            get {
#if  UNITY_EDITOR
                return _target.editorAsset as GameObject;
#endif
                return null;
            }
        }

        public Transform Parent => _parent ?? transform;

        public bool IsOpen => _asset != null;

        #endregion

        public virtual void Save()
        {
#if  UNITY_EDITOR
            if (!_asset) {
                return;
            }
            try {
                UnityEditor.PrefabUtility.ApplyPrefabInstance(_asset, UnityEditor.InteractionMode.AutomatedAction);
            }
            catch (Exception e) {
                GameLog.LogError(e);
            }
#endif
        }

        public void Close()
        {
            if (!_asset) {
                return;
            }
            Save();
            OnClose(_asset);
            DestroyImmediate(_asset,true);
            _asset = null;
        }

        public void Open()
        {
            
#if  UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            var target = _target.editorAsset;
            if (!target || _asset)
                return;
            _asset = UnityEditor.PrefabUtility.InstantiatePrefab(target, Parent) as GameObject;
            OnOpen(_asset);
#endif

        }

        protected virtual void OnOpen(GameObject asset)
        {
            
        }

        protected virtual void OnClose(GameObject asset)
        {
            
        }
        
    }
}
