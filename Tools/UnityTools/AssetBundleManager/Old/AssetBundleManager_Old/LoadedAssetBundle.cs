using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Tools.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old{
    
    // Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
    public class LoadedAssetBundle : IDisposable
    {
        private static Type _componentType = typeof(Component);
        private List<Object> _emptyObjects = new List<Object>();

        private string[] _allAssetsNames;
        private HashSet<Object> _loadedAssets = new HashSet<Object>();
        
        private Dictionary<string, Object> _loadedAssetsObjects = new Dictionary<string, Object>();

        private Dictionary<Type, List<Object>> _loadedAssetsObjectsTypes = new Dictionary<Type, List<Object>>();
        private Dictionary<string, Dictionary<Type, Object>> _loadedNameTypeAssetCashe =
            new Dictionary<string, Dictionary<Type, Object>>();

        #region constructor

        public LoadedAssetBundle(AssetBundle assetBundle)
        {
            if (!assetBundle)
                throw new ArgumentNullException("assetBundle", "AssetResource in LoadedAssetBundle is NULL");
            AssetBundle = assetBundle;
            ReferencedCount = 1;
            UpdateShaders();
        }

        #endregion

        #region public properties
            
        public AssetBundle AssetBundle { get; protected set; }

        public string BundleName
        {
            get { return AssetBundle.name; }
        }

        public string[] AllAssetsNames {
            get {

                if (_allAssetsNames == null) {
                    _allAssetsNames = AssetBundle.GetAllAssetNames();
                }

                return _allAssetsNames;
            }
        }

        public int ReferencedCount { get; set; }

        public IEnumerable<Object> CachedObjects {
            get {
                return _loadedAssets;
            }
        }

        #endregion

        #region public methods

        public void Dispose()
        {
            _loadedAssetsObjects.Clear();
            _loadedAssetsObjectsTypes.Clear();
            _loadedAssets.Clear();
            _loadedNameTypeAssetCashe.Clear();
            AssetBundle.Unload(true);
            AssetBundle = null;
        }

        #region async operations

        public IEnumerator LoadAllPrefabAssetsAsync<T>(Action<List<T>> callback)
            where T : Object
        {
            var type = typeof(T);
            if (_loadedAssetsObjectsTypes.ContainsKey(type))
            {
                if (callback != null) callback(GetCacheAssets<T>());
                yield break;
            }
            yield return LoadAssetsInternalAsync<GameObject>();
            var gameObjects = GetCacheAssets<GameObject>();

            var result = LoadComponents<T>(gameObjects);
            if(callback!=null)
                callback(result.OfType<T>().ToList());

        }

        public IEnumerator LoadPrefabAssetAsync<T>(Action<T> callback) where T : Object
        {
            yield return LoadAllPrefabAssetsAsync<T>(x => callback(x.FirstOrDefault()));
        }
        
        public IEnumerator LoadPrefabAssetAsync<T>(string assetName,Action<T> callback) where T : Object
        {
            GameObject asset = null;
            yield return LoadAssetByNameAsync<GameObject>(assetName, x => asset = x);
            if (!asset)
            {
                Debug.LogErrorFormat("LOAD NULL Game Object {0}",assetName);
                callback(null);
                yield break;
            }
            var component = asset.GetComponent<T>();
            callback(component);
        }

        public IEnumerator LoadAssetAsync<T>(Action<T> callback) where T : Object
        {
            if (IsComponent(typeof(T)))
            {
                yield return LoadPrefabAssetAsync(callback);
                yield break;
            }
            yield return LoadAssetsInternalAsync<T>();
            yield return LoadAllAssetsAsync<T>(x => callback(x.FirstOrDefault()));
        }

        public IEnumerator LoadAssetByNameAsync<T>(string name, Action<T> callback)
            where T : Object
        {
            if (IsComponent(typeof(T)))
            {
                yield return LoadPrefabAssetAsync(name,callback);
                yield break;
            }
            yield return LoadAssetInternalAsync<T>(name);
            var asset = GetCacheAsset<T>(name);
            callback(asset);
        }

        public IEnumerator LoadAssetAsync(string name)
        {
            var request = AssetBundle.LoadAssetAsync(name);
            yield return request;
            AddAsset(name, request.asset);
        }

        public IEnumerator LoadAllAssetsAsync<T>(Action<List<T>> callback)
            where T : Object
        {
            var type = typeof(T);
            if (IsComponent(type))
            {
                yield return LoadAllPrefabAssetsAsync<T>(callback);
                yield break;
            }
            yield return LoadAssetsInternalAsync<T>();
            var result = GetCacheAssets(type);
            if(callback != null) callback(result.OfType<T>().ToList());
        }

        public IEnumerator LoadAllAssetsAsync(Type type,Action<List<Object>> callback)
        {
            yield return LoadAssetsInternalAsync(type);
            var result = GetCacheAssets(type);
            if (callback != null) callback(result);
        }
        
        #endregion


        #region sync operations

        private List<Object> LoadPrefabAssets<T>()
            where T : Object
        {
            var type = typeof(T);
            var result = new List<Object>();

            if (_loadedAssetsObjectsTypes.TryGetValue(type,out result) == true)
                return result;

            var items = LoadAssets<GameObject>();
            return LoadComponents<T>(items);
        }

        public Object LoadAsset(string name)
        {
            Object result = null;
            if (!_loadedAssetsObjects.ContainsKey(name))
            {
                result = AssetBundle.LoadAsset(name);
                if (result == null)
                {
                    Debug.LogErrorFormat("Loading asset {0} from Bundle {1} Failed", name, AssetBundle.name);
                    return null;
                }
                AddAsset(name, result);
            }
            result = _loadedAssetsObjects[name];
            return result;
        }

        public T LoadAsset<T>() where T : Object
        {
            if (!AssetBundle)
            {
                throw new NullReferenceException("AssetResource is NULL");
            }
            return LoadAssets<T>().FirstOrDefault();
        }

        public T LoadAsset<T>(string assetName) where T : Object {
            
            var asset = GetCacheAsset<T>(assetName);

            if (asset) return asset;

            var targetType = typeof(T);
            if (_componentType.IsAssignableFrom(targetType)) {
                var gameObject = AssetBundle.LoadAsset<GameObject>(assetName);
                asset = gameObject.GetComponent<T>();
            }
            else {
                asset = AssetBundle.LoadAsset<T>(assetName); 
            }

            AddAsset(assetName, targetType, asset);
            return asset;
        }

        public List<T> LoadAssets<T>()
            where T : Object
        {
            if (!AssetBundle)
            {
                throw new NullReferenceException("AssetResource is NULL");
            }
            var items = LoadAssetsInternal<T>();
            return items.OfType<T>().ToList();
        }

        public T LoadAssetByTypeName<T>() where T : Object
        {
            return LoadAsset(typeof(T).Name) as T;
        }

        public Object LoadAssetByName(string assetName)
        {
            return AssetBundle.LoadAsset(assetName);
        }

        #endregion

        #endregion

        #region private methods


        private void UpdateShaders() {

            //load all shaders to memory
            AssetBundle.LoadAllAssets<Shader>();
            var materials = AssetBundle.LoadAllAssets<Material>();
            for (int i = 0; i < materials.Length; i++) {
                var material = materials[i];
                //ShaderCompiler
                material.shader = Shader.Find(material.shader.name); //hack to force 
                AddAsset(material.name, material.GetType(), material);
            }
            
        }


        #region cache methods

        private void AddAsset(string name, Object asset)
        {
            if (asset == null || string.IsNullOrEmpty(name)) return;
            var type = asset.GetType();
            AddAsset(name, type, asset);
        }

        private void AddAsset(string name, Type type, Object asset)
        {
            if (!asset)
            {
                Debug.LogErrorFormat("Try add to cache NULL asset : {0} type {1}",name,type);
                return;
            }
            //is valid asset
            if (asset == null ||
                string.IsNullOrEmpty(name) ||
                _loadedAssets.Contains(asset))
                return;
            if (!_loadedAssetsObjectsTypes.ContainsKey(type))
                _loadedAssetsObjectsTypes[type] = new List<Object>();
            _loadedAssets.Add(asset);
            _loadedAssetsObjectsTypes[type].Add(asset);
            _loadedAssetsObjects[name] = asset;
            if (!_loadedNameTypeAssetCashe.ContainsKey(name))
                _loadedNameTypeAssetCashe[name] = new Dictionary<Type, Object>();
            _loadedNameTypeAssetCashe[name][type] = asset;
        }

        private void AddAssets(Type type, Object[] asset)
        {
            if (asset == null || asset.Length == 0) return;
            for (var i = 0; i < asset.Length; i++)
            {
                AddAsset(asset[i].name, type, asset[i]);
            }
        }

        private List<Object> GetCacheAssets(Type type)
        {
            return _loadedAssetsObjectsTypes.ContainsKey(type) ? _loadedAssetsObjectsTypes[type] : _emptyObjects;
        }

        private List<T> GetCacheAssets<T>()
            where T : Object
        {
            var type = typeof(T);
            if (!_loadedAssetsObjectsTypes.ContainsKey(type))
                return new List<T>();
            var result = _loadedAssetsObjectsTypes[type];
            return result.OfType<T>().ToList();
        }

        private T GetCacheAsset<T>(string assetName)
            where T : Object {

            Dictionary<Type, Object> items = null;

            if (!_loadedNameTypeAssetCashe.TryGetValue(assetName,out items))
                return null;

            var type = typeof(T);
            Object asset = null;
            if (!items.TryGetValue(type, out asset))
                return null;
            return asset as T;
        }

        #endregion

        private List<Object> LoadComponents<T>(List<GameObject> items) where T : Object {

            var targetType = typeof(T);

            AddComponentsToCache(items,targetType);

            List<Object> result = null;

            if (_loadedAssetsObjectsTypes.TryGetValue(targetType, out result) == true)
                return result;

            return _emptyObjects;
        }

        private void AddComponentsToCache(List<GameObject> items,Type componentType) {

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                if (!item) continue;

                var components = item.GetComponents(componentType);
                
                if (components.Length == 0) continue;

                AddAssets(componentType, components);
            }

        }

        /// <summary>
        /// sync loading objectts by type from bundle
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <returns></returns>
        private List<Object> LoadAssetsInternal<T>()
            where T : Object
        {
            var type = typeof(T);
            if (_loadedAssetsObjectsTypes.ContainsKey(type))
                return GetCacheAssets(type);
            if (IsComponent(type) == true) {
                return LoadPrefabAssets<T>();
            }
            var result = AssetBundle.LoadAllAssets<T>();
            if (result.Length == 0)
            {
                Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed",
                    type.Name, AssetBundle.name);
            }
            else
            {
                AddAssets(type, result);
            }
            return GetCacheAssets(type);
        }


        private IEnumerator LoadAssetsInternalAsync<T>()
        {
            var type = typeof(T);
            if (_loadedAssetsObjectsTypes.ContainsKey(type))
            {
                yield break;
            }
            var result = AssetBundle.LoadAllAssetsAsync<T>();
            yield return result;
            if (result.allAssets.Length == 0)
            {
                Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed",
                    type.Name, AssetBundle.name);
            }
            AddAssets(type, result.allAssets);
        }

        private IEnumerator LoadAssetsInternalAsync(Type type)
        {
            if (_loadedAssetsObjectsTypes.ContainsKey(type))
            {
                yield break;
            }
            var result = AssetBundle.LoadAllAssetsAsync(type);
            yield return result;
            if (result.allAssets.Length == 0)
            {
                Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed",
                    type.Name, AssetBundle.name);
            }
            AddAssets(type, result.allAssets);
        }

        private IEnumerator LoadAssetInternalAsync<T>(string assetName)
            where T : Object
        {
            var type = typeof(T);
            var asset = GetCacheAsset<T>(assetName);
            if (asset)
            {
                yield break;
            }
			var result = AssetBundle.LoadAssetAsync<T>(assetName);
            yield return result;
            AddAsset(assetName, type, result.asset);
        }

        private bool IsComponent(Type targetType)
        {
            return _componentType.IsAssignableFrom(targetType);
        }

        #endregion

    }
}