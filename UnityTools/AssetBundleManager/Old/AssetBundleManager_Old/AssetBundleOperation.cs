using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Tools.UnityTools.ObjectPool.Scripts;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Tools.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old{

    public class AssetBundleOperation : IAssetProvider
    {
        private readonly bool _loadDpendencies;
        private string _path;
        private LoadedAssetBundle _assetBundleResource;
        
        #region public properties

        public string AssetBundleName { get; protected set; }

        public string AssetBundlePath { get; protected set; }

        public string[] AllAssetsNames {
            get { return _assetBundleResource.AllAssetsNames; }
        }

        public bool IsDependenciesLoaded { get; protected set; }

        public LoadedAssetBundle LoadedAssetBundle {
            get { return _assetBundleResource; }
        }

        #endregion

        #region constructor
        
        public AssetBundleOperation(string assetName,string assetBundlePath,bool loadDependencies)
        {
            _loadDpendencies = loadDependencies;
            AssetBundleName = assetName;
            AssetBundlePath = assetBundlePath;      
        }

        #endregion

        #region public methods

        public void Load() {
            if (_assetBundleResource != null)
                return;
            _assetBundleResource = AssetsBundleLoader.LoadAssetBundle(AssetBundleName, _loadDpendencies);
            IsDependenciesLoaded = _loadDpendencies;
        }

        public IEnumerator Execute()
        {
            if(_assetBundleResource!=null)
                yield break;
            yield return AssetsBundleLoader.LoadAssetBundleAsync(AssetBundleName,_loadDpendencies);
            _assetBundleResource = AssetsBundleLoader.GetLoadedAssetBundle(AssetBundleName);
            IsDependenciesLoaded = _loadDpendencies;
        }

        public T LoadAsset<T>() where T : Object
        {
            return _assetBundleResource.LoadAsset<T>();
        }

        public T LoadAsset<T>(string assetName) where T : Object {
            return _assetBundleResource.LoadAsset<T>(assetName);
        }

        public List<T> LoadAssets<T>() where T : Object
        {
            return _assetBundleResource.LoadAssets<T>();
        }

        public T LoadAssetByTypeName<T>() where T : Object
        {
            return _assetBundleResource.LoadAssetByTypeName<T>();
        }

        public Object LoadAssetByName(string assetName)
        {
            return _assetBundleResource.LoadAssetByName(assetName);
        }

        #region async

        public IEnumerator LoadAssetByTypeNameAsync<T>(Action<T> callback)
            where T : Component
        {
            var name = typeof(T).Name;
            yield return _assetBundleResource.LoadAssetByNameAsync<GameObject>(name,asset =>
            {
                var assetObject = asset.Spawn<T>();
                callback(assetObject);
            });
        }

        public IEnumerator LoadAssetAsync<T>(Action<T> callback) where T : Object
        {
            yield return _assetBundleResource.LoadAssetAsync<T>(callback);
        }

        public IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback) where T : Object
        {
            yield return _assetBundleResource.LoadAllAssetsAsync<T>(callback);
        }

        public IEnumerator LoadAssetsAsync(Type type,Action<List<Object>> callback) 
        {
            yield return _assetBundleResource.LoadAllAssetsAsync(type,callback);
        }

        public IEnumerator LoadPrefabAssetsAsync<T>(Action<List<T>> callback) where T : Component
        {
            yield return _assetBundleResource.LoadAllPrefabAssetsAsync<T>(callback);
        }

        public IEnumerator LoadPrefabAssetAsync<T>(Action<T> callback) where T : Component
        {          
            yield return _assetBundleResource.LoadPrefabAssetAsync<T>(callback);
        }
        
        public IEnumerator LoadPrefabAssetAsync<T>(string name,Action<T> callback) where T : Component
        {      
            yield return _assetBundleResource.LoadPrefabAssetAsync<T>(name,callback);
        }

        public IEnumerator LoadAssetByNameAsync<T>(string assetName, Action<T> action)
            where T : Object {
            
            yield return _assetBundleResource.LoadAssetByNameAsync<T>(assetName,action);   
            
        }
        
        #endregion

        #endregion

    }
}
