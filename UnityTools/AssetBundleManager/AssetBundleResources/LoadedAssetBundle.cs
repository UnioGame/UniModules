using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tools.UnityTools.ProfilerTools;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources
{
    // Loaded assetBundle contains the references count which can be used to unload dependent assetBundles automatically.
    public class LoadedAssetBundle : IAssetBundleResource {

        private static Type _componentType = typeof(Component);
        private List<Object> _emptyObjects = new List<Object>();
        private AssetBundle _assetBundle;
        
        private string[] _allAssetsNames;

        private string[] _dependencies;
        private HashSet<Object> _loadedAssets = new HashSet<Object>();
        
        private Dictionary<string, Object> _cachedAssets = new Dictionary<string, Object>();
        private Dictionary<string, List<Object>> _cachedAssetWithSubAssets = new Dictionary<string, List<Object>>();
        private Dictionary<Type, List<Object>> _loadedAssetsObjectsTypes = new Dictionary<Type, List<Object>>();
        private Dictionary<string, Dictionary<Type, Object>> _loadedNameTypeAssetCashe =
            new Dictionary<string, Dictionary<Type, Object>>();

        #region constructor

        public void Initialize(AssetBundle assetBundle)
        {
            if (!assetBundle) {
                throw new ArgumentNullException("assetBundle", "AssetResource in LoadedAssetBundle is NULL");
            }

            _assetBundle = assetBundle;
            BundleName = _assetBundle ? _assetBundle.name : null;
            ReferencedCount = 1;
            UpdateShaders();
        }

        #endregion

        #region public properties

        public string BundleName { get; protected set; }

        public IDictionary<string, Object> CachedAssets {
            get { return _cachedAssets; }
        }

        public string[] AllAssetsNames {
            get {

                if (_allAssetsNames == null) {
                    _allAssetsNames = _assetBundle.GetAllAssetNames();
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

        public void Release()
        {
            _cachedAssets.Clear();
            _loadedAssetsObjectsTypes.Clear();
            _loadedAssets.Clear();
            _loadedNameTypeAssetCashe.Clear();
            _cachedAssetWithSubAssets.Clear();
            _assetBundle = null;
        }
        
        public bool TryUnload(bool unloadForceMode) {
            if (ReferencedCount <= 0) {
                return Unload(false, unloadForceMode);
            }

            return false;
        }

        public bool Unload(bool forceUnload,bool forceMode) {
            if (!_assetBundle)
                return true;
            if (forceUnload || --ReferencedCount <= 0) {
                _assetBundle.Unload(forceMode);
                
                GameLog.LogResource(string.Format("UNLOAD BUNDLE {0} FORCE {1} MODE {2}",
                    this.BundleName,forceUnload,forceMode));
                
                return true;
            }
            return false;
        }

        #region async operations

        public IEnumerator LoadAssetAsync<T>(Action<T> callback) where T : Object
        {
            if (IsComponent(typeof(T)))
            {
                GameLog.LogErrorFormat("Trying to load Component from bundle {0} by Type {1}", BundleName,typeof(T).Name);
                yield break;
            }
            yield return LoadAssetAsync<T>(callback);
        }

        public IEnumerator LoadAssetAsync<T>(string assetName, Action<T> callback)
            where T : Object {

            T asset = null;
            if (IsComponent(typeof(T))) {
                GameObject gameObject = null;
                yield return LoadAssetInternalAsync<GameObject>(assetName, x => gameObject = x);
                if (!gameObject)
                {
                    Debug.LogErrorFormat("LOAD NULL Game Object {0}", assetName);
                    callback(null);
                    yield break;
                }
                asset = gameObject.GetComponent<T>();
            }
            else {
                yield return LoadAssetInternalAsync<T>(assetName,x => asset = x);
            }
            if(callback!=null)
                callback(asset);
        }

        public IEnumerator LoadAllAssetsAsync<T>(Action<List<T>> callback)
            where T : Object
        {
            var type = typeof(T);
            if (IsComponent(type))
            {
                //yield return LoadAllPrefabAssetsAsync<T>(callback);
                GameLog.LogErrorFormat("Trying to load Component from bundle {0} by Type {1}", BundleName, typeof(T).Name);
                yield break;
            }
            yield return LoadAssetsInternalAsync<T>(callback);
        }

        public IEnumerator LoadAllAssetsAsync(Type type,Action<List<Object>> callback)
        {
            yield return LoadAssetsInternalAsync(type,callback);
            var result = GetCacheAssets(type);
            if (callback != null) callback(result);
        }

        public IEnumerator LoadAssetWithSubAssetsAsync(string assetName, Action<List<Object>> callback) {
            yield return LoadAssetWithSubAssetsInternalAsync(assetName, callback);
        }

        #endregion

        #region sync operations

        public Object LoadAsset(string assetName) {
            
            var result = LoadAsset<Object>(assetName);
            return result;

        }

        public T LoadAsset<T>() where T : Object
        {
            if (!_assetBundle)
            {
                throw new NullReferenceException("AssetResource is NULL");
            }
            return LoadAssets<T>().FirstOrDefault();
        }

        public T LoadAsset<T>(string assetName) where T : Object {
            
            var asset = GetCacheAsset<T>(assetName);

            if (!asset) {
                var targetType = typeof(T);
                if (_componentType.IsAssignableFrom(targetType)) {
                    var gameObject = _assetBundle.LoadAsset<GameObject>(assetName);
                    asset = gameObject.GetComponent<T>();
                }
                else {
                    asset = _assetBundle.LoadAsset<T>(assetName);
                }
                AddAsset(assetName, targetType, asset);
            }

            ReferencedCount++;
            return asset;
        }

        public List<T> LoadAssets<T>()
            where T : Object
        {
            var items = LoadAssetsInternal<T>();
            return items.OfType<T>().ToList();
        }

        public List<Object> LoadAssetWithSubAssets(string assetName) {
            return LoadAssetWithSubAssetsInternal(assetName);
        }

        public T LoadAssetByTypeName<T>() where T : Object
        {
            return LoadAsset(typeof(T).Name) as T;
        }

        #endregion

        #endregion

        #region private methods

        private IEnumerator LoadAssetAsync(string assetName)
        {
            var request = _assetBundle.LoadAssetAsync(assetName);
            yield return request;
            AddAsset(assetName, request.asset);
        }

        private void UpdateShaders() {

            //load all shaders to memory
            _assetBundle.LoadAllAssets<Shader>();
            var materials = _assetBundle.LoadAllAssets<Material>();
            for (var i = 0; i < materials.Length; i++)
            {
                var material = materials[i];
                //ShaderCompiler
                material.shader = Shader.Find(material.shader.name); //apply shader to material
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

            var isLoaded = _loadedAssets.Contains(asset);
            //is valid asset
            if (asset == null || string.IsNullOrEmpty(name))
                return;
            
            if (!_loadedAssetsObjectsTypes.ContainsKey(type))
                _loadedAssetsObjectsTypes[type] = new List<Object>();
            
            _loadedAssets.Add(asset);
            _cachedAssets[name] = asset;
            
            if(!isLoaded)
                _loadedAssetsObjectsTypes[type].Add(asset);
            
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

        private List<Object> GetCacheAssets(Type type) {
            List<Object> assets = null;
            var result = _loadedAssetsObjectsTypes.TryGetValue(type, out assets) ? assets : _emptyObjects ;
            return result;
        }

        private List<T> GetCacheAssets<T>()
            where T : Object
        {
            var type = typeof(T);
            List<Object> result = null;
            if (!_loadedAssetsObjectsTypes.TryGetValue(type, out result))
                return new List<T>();
            return result.OfType<T>().ToList();
        }

        private Object GetCacheAsset(Type type) {

            List<Object> assets = null;
            _loadedAssetsObjectsTypes.TryGetValue(type, out assets);
            return assets == null ? null : assets.FirstOrDefault();

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

        private List<T> LoadComponents<T>(List<GameObject> items) where T : Object {

            var targetType = typeof(T);

            AddComponentsToCache(items,targetType);

            List<Object> result = null;

            if (_loadedAssetsObjectsTypes.TryGetValue(targetType, out result) == true)
                return result.OfType<T>().ToList();

            return new List<T>();
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
            var result = GetCacheAssets(type);
            
            if (result.Count == 0 && IsComponent(type) == true) {
                Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed", type.Name, _assetBundle.name);

                //result = LoadPrefabAssets<T>();
            }

            if (result.Count == 0) {
                var bundleAssets = _assetBundle.LoadAllAssets<T>();
                if (bundleAssets.Length == 0) {
                    Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed", type.Name, _assetBundle.name);
                    return result;
                }
                AddAssets(type, bundleAssets);
                result = new List<Object>(bundleAssets);
            }

            ReferencedCount++;
            return result;
        }

        private IEnumerator LoadAssetsInternalAsync<T>(Action<List<T>> callback)
        {
            var type = typeof(T);
            var result = GetCacheAssets(type);
            if (result.Count == 0) {
                var request = _assetBundle.LoadAllAssetsAsync<T>();
                yield return request;
                if (request.allAssets.Length == 0) {
                    Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed", type.Name, _assetBundle.name);
                }
                AddAssets(type, request.allAssets);
                result = GetCacheAssets(type);
            }

            ReferencedCount++;
            if (callback != null)
                callback(result.OfType<T>().ToList());
        }

        private IEnumerator LoadAssetsInternalAsync(Type type,Action<List<Object>> callback) {
            var assets = GetCacheAssets(type);
            if (assets == null || assets.Count==0) {
                var result = _assetBundle.LoadAllAssetsAsync(type);
                yield return result;
                if (result.allAssets.Length == 0) {
                    Debug.LogErrorFormat("Loading asset TYPE {0} from Bundle {1} Failed", type.Name, _assetBundle.name);
                }

                assets = result.allAssets.ToList();
                AddAssets(type, result.allAssets);
            }

            ReferencedCount++;
            if (callback != null)
                callback(assets);
        }

        private IEnumerator LoadAssetInternalAsync<T>(string assetName, Action<T> callback)
            where T : Object
        {
            var type = typeof(T);
            var asset = GetCacheAsset<T>(assetName);
            if (!asset) {

                var result = _assetBundle.LoadAssetAsync<T>(assetName);
                yield return result;
                asset = result.asset as T;
                AddAsset(assetName, type, result.asset);
                
            }

            if (asset) ReferencedCount++;
            if(callback!=null)
                callback(asset);
        }

        private List<Object> LoadAssetWithSubAssetsInternal(string assetName) {
            List<Object> subAssets;
            if (_cachedAssetWithSubAssets.TryGetValue(assetName, out subAssets))
                return subAssets;
            var assets = _assetBundle.LoadAssetWithSubAssets(assetName);
            subAssets = new List<Object>(assets);
            _cachedAssetWithSubAssets[assetName] = subAssets;
            return subAssets;
        }

        private IEnumerator LoadAssetWithSubAssetsInternalAsync(string assetName, Action<List<Object>> callback)
        {
            List<Object> subAssets;
            if (_cachedAssetWithSubAssets.TryGetValue(assetName, out subAssets) == false) {
                var request = _assetBundle.LoadAssetWithSubAssetsAsync(assetName);
                yield return request;
                subAssets = new List<Object>(request.allAssets);
                _cachedAssetWithSubAssets[assetName] = subAssets;
            }
            if(callback!=null)
                callback(subAssets);
        }

        private T LoadAssetInternal<T>(string assetName)
            where T : Object
        {
            var type = typeof(T);
            var asset = GetCacheAsset<T>(assetName);
            if (asset == null) {
                asset = _assetBundle.LoadAsset<T>(assetName);
                AddAsset(assetName, type, asset);
            }
            if(asset)
                ReferencedCount++;

            return asset;
        }

        private bool IsComponent(Type targetType)
        {
            return _componentType.IsAssignableFrom(targetType);
        }

        #endregion

    }
}