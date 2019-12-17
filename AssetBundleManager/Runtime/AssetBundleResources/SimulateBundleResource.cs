namespace UniGreenModules.AssetBundleManager.Runtime.AssetBundleResources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UniCore.Runtime.ProfilerTools;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class SimulateBundleResource : IAssetBundleResource
    {
        private static Type _componentType = typeof(Component);
        private static Type _gameObjectType = typeof(GameObject);

        private bool _initialized = false;
        private Dictionary<string, Dictionary<Type, List<Object>>> _namedAssets;
        private Dictionary<Type, List<Object>> _assets;
        private Dictionary<string, List<string>> _assetsMap;
        private List<Component> _components = new List<Component>();
        private Dictionary<string,Object> _cachedAssets = new Dictionary<string, Object>();
        private Dictionary<string,List<Object>> _cachedassetsWithSubAssets = new Dictionary<string, List<Object>>();

        public string BundleName { get; protected set; }
        public string[] AllAssetsNames { get; protected set; }
        public string[] Dependencies { get; protected set; }
        public int ReferencedCount { get; set; }

        #region constructor

        public void Initialize(string assetBundleName)
        {

            BundleName = Path.GetFileNameWithoutExtension(assetBundleName);
            if (string.IsNullOrEmpty(BundleName)) {
                GameLog.LogErrorFormat("SimulateBundleResource assetBundleName is empty", assetBundleName);
                return;
            }
            _namedAssets = new Dictionary<string, Dictionary<Type, List<Object>>>();
            _assetsMap = new Dictionary<string, List<string>>();
            _assets = new Dictionary<Type, List<Object>>();

#if UNITY_EDITOR

            AllAssetsNames = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(BundleName);
            Dependencies =  new string[0];//UnityEditor.AssetDatabase.GetAssetBundleDependencies(assetBundleName,false);

            if (AllAssetsNames.Length == 0)
            {
                GameLog.LogErrorFormat("There is no asset in Bundle {0}", BundleName);
                return;
            }

            InitializeAssets(AllAssetsNames);
#endif
        }

        #endregion

        #region public methods

        public void Dispose() {
            AllAssetsNames = null;
            Unload(true,true);
        }

        public IDictionary<string, Object> CachedAssets {
            get { return _cachedAssets; }
        }

        public IEnumerator LoadAssetAsync<T>(Action<T> action) where T : Object {
            var asset = LoadAsset<T>();
            action(asset);
            yield break;
        }

        public IEnumerator LoadAssetAsync<T>(string assetName, Action<T> callback) where T : Object {
            var asset = LoadAssetByName<T>(assetName);
            callback(asset);
            yield break;
        }

        public IEnumerator LoadAssetObjectAsync(string assetName,Action<Object> action) {
            var asset = LoadAssetByName(assetName);
            action(asset);
            yield break;
        }

        public IEnumerator LoadAllAssetsAsync<T>(Action<List<T>> callback) where T : Object {
            var assets = LoadAssets<T>();
            callback(assets);
            yield break;
        }

        public IEnumerator LoadAllAssetsAsync(Type type, Action<List<Object>> callback) {
            List<Object> assets = null;
            _assets.TryGetValue(type, out assets);
            callback(assets);
            yield break;
        }

        public IEnumerator LoadAssetWithSubAssetsAsync(string assetName, Action<List<Object>> callback) {
            var assets = LoadAssetWithSubAssets(assetName);
            if (callback != null)
                callback(assets);
            yield break;
        }

        public Object LoadAsset(string assetName) {
            return LoadAssetByName(assetName);
        }

        #region sync

        public T LoadAsset<T>() where T : Object
        {
            LoadAssetsInternal();
            var type = typeof(T);
            if (!_assets.ContainsKey(type))
                return null;
            return _assets[type].FirstOrDefault() as T;
        }

        public T LoadAsset<T>(string assetName) where T : Object {
            return LoadEditorAsset<T>(assetName);
        }

        public List<T> LoadAssets<T>() where T : Object {
            LoadAssetsInternal();
            var type = typeof(T);
            if (!_assets.ContainsKey(type))
                return null;
            return _assets[type].OfType<T>().ToList();
        }

        public List<Object> LoadAssetWithSubAssets(string assetName) {
            List<Object> assets;
            if (_cachedassetsWithSubAssets.TryGetValue(assetName, out assets))
                return assets;
            assets = new List<Object>();
#if UNITY_EDITOR
            List<string> assetPaths;
            if (_assetsMap.TryGetValue(assetName, out assetPaths) == false)
                return null;
            var path = assetPaths.FirstOrDefault();
            var loadAssets = UnityEditor.AssetDatabase.LoadAllAssetsAtPath(path);
            assets.AddRange(loadAssets);
            _cachedassetsWithSubAssets[assetName] = assets;
#endif
            return assets;
        }

        public T LoadAssetByTypeName<T>() where T : Object {
            var assetName = typeof(T).Name;
            return LoadAssetByName<T>(assetName);
        }

        public T LoadAssetByName<T>(string resourceName) where T : Object
        {
            return LoadEditorAsset<T>(resourceName);
        }

        public Object LoadAssetByName(string assetName) {
            throw new NotImplementedException();
        }

        public bool Unload(bool forceUnload, bool forceMode) {

            //if (forceUnload || --ReferencedCount <= 0) {
            //    _namedAssets.Clear();
            //    _assets.Clear();
            //    _assetsMap.Clear();
            //    return true;
            //}

            return false;
        }

        public bool TryUnload(bool unloadForceMode) {
            return false;
        }

        #endregion

        #endregion

        [Conditional("LOGS_ENABLED")]
        private void LogDependencies(string[] dependeies) {
            //todo move to conditional method
            var abDependencies = string.Format("AssetBundle [{0}] dependencies : ", BundleName);
            for (var i = 0; i < Dependencies.Length; i++)
                abDependencies = string.Format("{0} : {1}", abDependencies, Dependencies[i]);
            GameLog.Log(abDependencies);

        }

        private void InitializeAssets(string[] assetPaths)
        {
            foreach (var assetPath in assetPaths)
            {

                var assetName = Path.GetFileNameWithoutExtension(assetPath);

                _assetsMap[assetPath] = new List<string>();

                if (!_assetsMap.ContainsKey(assetName))
                {
                    _assetsMap[assetName] = new List<string>();
                }

                _assetsMap[assetName].Add(assetPath);
                _assetsMap[assetPath].Add(assetPath);
            }

        }

        private void LoadAssetsInternal()
        {
            GameProfiler.BeginSample("Simulate.LoadAssetsInternal " + BundleName);

            if (_initialized) return;
#if  UNITY_EDITOR
            foreach (var assetPath in AllAssetsNames)
            {
                LoadAssetData(assetPath);
            }
#endif
            _initialized = true;

            GameProfiler.EndSample();
        }

        private void LoadAssetData(string assetPath)
        {
#if UNITY_EDITOR
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            var gameObject = asset as GameObject;
            if (gameObject != null)
            {
                var components = gameObject.GetComponents(typeof(Component));
                for (int i = 0; i < components.Length; i++)
                {
                    AddAsset(components[i],assetPath);
                }

                gameObject.SetActive(false);
            }
            AddAsset(asset, assetPath);
            if (asset is Texture2D)
            {
                var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                AddAsset(sprite, assetPath);
            }
#endif

        }

        private void AddAsset(Object asset, string assetPath)
        {
            if (asset == null) return;

            var type = asset.GetType();
            List<Object> typeAssets = null;
            if (!_assets.TryGetValue(type,out typeAssets))
            {
                typeAssets = new List<Object>();
                _assets[type] = typeAssets;
            }
            _assets[type].Add(asset);

            Dictionary<Type, List<Object>> assets = null;
            if (!_namedAssets.TryGetValue(assetPath,out assets))
            {
                assets = new Dictionary<Type, List<Object>>();
                _namedAssets[assetPath] = assets;
            }

            List<Object> typeObjects = null;
            if(!assets.TryGetValue(type,out typeObjects))
            {
                typeObjects = new List<Object>();
                assets[type] = typeObjects;
            }

            typeObjects.Add(asset);

        }

        private T LoadEditorAsset<T>(string assetName) where T : Object
        {
#if UNITY_EDITOR
            if (_assetsMap.ContainsKey(assetName) == false)
            {
                GameLog.LogErrorFormat("Asset bundle [{0}] operation LoadAsset: [{1}] in NULL", BundleName, assetName);
                return null;
            }

            var targetType = typeof(T);
            var isComponent = _componentType.IsAssignableFrom(targetType);
            var resultAsset = GetCacheAsset<T>(assetName);
            Object asset = null;

            if (resultAsset == null)
            {
                GameProfiler.BeginSample("Simulate.LoadEditorAsset<T>");

                var assets = _assetsMap[assetName];
                for (int i = 0; i < assets.Count; i++)
                {

                    var path = assets[i];
                    asset = isComponent
                        ? UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path) as Object
                        : UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset == null) {
                        var loadedResource = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(path);
                        asset = loadedResource as T;
                    }
                    if (asset == null) {
                        continue;
                    }

                    //add already loaded asset to cache
                    AddAsset(asset, path);

                    var gameObject = asset as GameObject;
                    var resultComponent = AddComponents<T>(gameObject,path);
                    if (resultComponent)
                        resultAsset = resultComponent;

                    var targetTypeAsset = asset as T;
                    if (targetTypeAsset != null) {
                        resultAsset = targetTypeAsset;
                    }
                }

                GameProfiler.EndSample();
                
                if (resultAsset == null)  {
                    GameLog.LogErrorFormat("Requested resource with name: {0} from bundle {1} is NULL", assetName,
                        BundleName);
                    return null;
                }
            }

            resultAsset = MakeInstance<T>(resultAsset,isComponent);
 
            if (resultAsset == null)
            {
                GameLog.LogErrorFormat("Requested resource with name: {0} from bundle {1} is NULL", assetName, BundleName);
                return null;
            }

            return resultAsset;
#else
            return null;
#endif
        }

        private T MakeInstance<T>(T asset, bool isComponent) where T : Object {

            T resultAsset = null;
            if (asset is GameObject || isComponent)
            {
                var component = asset as Component;
                var targetObject = asset as GameObject;
                var go = component != null ? component.gameObject : targetObject;
                var position = go.transform.position;
                var rotation = go.transform.rotation;
                var resultGameObject = ObjectPool.Spawn(go, position, rotation, false);

                AssetsInstanceMap.Register(resultGameObject, go);

                resultAsset = isComponent ? resultGameObject.GetComponent<T>() : resultGameObject as T;
            }
            else
            {
				if (Application.isEditor == true && asset is Texture) {
					resultAsset = asset;
				} else {
					resultAsset = Object.Instantiate(asset);
				}
            }

            AssetsInstanceMap.Register(resultAsset, asset);
            return resultAsset;
        }

        private T AddComponents<T>(GameObject gameObject,string path) where T : Object
        {
            T result = null;
            _components.Clear();
            if (gameObject != null)
            {
                gameObject.GetComponents(_components);
                for (int i = 0; i < _components.Count; i++)
                {
                    var component = _components[i];
                    var resultComponent = component as T;
                    if (resultComponent)
                    {
                        result = resultComponent;
                    }
                    AddAsset(component, path);
                }
            }

            return result;
        }

        private T GetCacheAsset<T>(string resourceName) where T : Object
        {
            if (string.IsNullOrEmpty(resourceName)) return null;

            if (!_namedAssets.ContainsKey(resourceName))
                return null;
            var namedItems = _namedAssets[resourceName];
            var targetType = typeof(T);

            if (_componentType.IsAssignableFrom(targetType) && 
                namedItems.ContainsKey(_gameObjectType))
            {
                var targetObject = namedItems[_gameObjectType].FirstOrDefault() as GameObject;
                return targetObject ? targetObject.GetComponent<T>() : null;
            }

            if (!namedItems.ContainsKey(targetType))
            {
                var foundType = namedItems.FirstOrDefault(x => targetType.IsAssignableFrom(x.Key)).Key;
                if (foundType == null) return null;
                return namedItems[foundType].FirstOrDefault() as T;
            }
            var asset = namedItems[targetType].FirstOrDefault();
            return asset as T;
        }

        public void Release() {
            _initialized = false;
            _namedAssets.Clear();
            _assets.Clear();
            _assetsMap.Clear();
            _components.Clear();
            BundleName = string.Empty;
            AllAssetsNames = null;
            Dependencies = null;
            ReferencedCount = 0;
            
        }

    }
}
