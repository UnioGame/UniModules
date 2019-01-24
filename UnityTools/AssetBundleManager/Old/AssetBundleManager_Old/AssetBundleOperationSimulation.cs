#if UNITY_EDITOR
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UniModule.UnityTools.ProfilerTools;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.AssetBundleManager.Old.AssetBundleManager_Old
{
    public class AssetBundleOperationSimulation : IAssetProvider {

        private const string loadSimulationAsset = "LoadOperationSimulation";
        private static Type _componentType = typeof(Component);
        private static Type _gameObjectType = typeof(GameObject);
        private static bool _useInstances = true;

        private string[] _assetPaths;
        private Dictionary<string, Dictionary<Type, Object>> _namedAssets;
        private Dictionary<Type, List<Object>> _assets;
        private Dictionary<string, List<string>> _assetsMap;
        private string _bundleName;

        public string[] AllAssetsNames
        {
            get
            {
                return _assetPaths;
            }
        }

        #region constructor

        public AssetBundleOperationSimulation(string assetBundleName)
        {

            _bundleName = assetBundleName;
            _namedAssets = new Dictionary<string, Dictionary<Type, Object>>();
            _assetsMap = new Dictionary<string, List<string>>();
            _assets = new Dictionary<Type, List<Object>>();

#if UNITY_EDITOR
            _assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            if (_assetPaths.Length == 0)
            {
                GameLog.LogErrorFormat("There is no asset in Bundle {0}", assetBundleName);
                return;
            }
            var dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, false);
            //todo move to conditional method
            var abDependencies = string.Format("AssetBundle [{0}] dependencies : ", assetBundleName);
            for (var i = 0; i < dependencies.Length; i++)
                abDependencies = string.Format("{0} : {1}", abDependencies, dependencies[i]);
            GameLog.Log(abDependencies);

            if (Application.isEditor == false || Application.isPlaying)
                InitializeAssets(_assetPaths);

#endif
        }

        #endregion

        public IEnumerator Execute()
        {
            yield break;
        }

        public void Load()
        {
            return;
        }

#if UNITY_EDITOR

        public T LoadAsset<T>() where T : Object
        {
            var type = typeof(T);
            if (!_assets.ContainsKey(type))
                return null;
            return _assets[type].FirstOrDefault() as T;
        }

        public List<T> LoadAssets<T>() where T : Object
        {
            var type = typeof(T);
            if (!_assets.ContainsKey(type))
                return null;
            return _assets[type].OfType<T>().ToList();
        }

        public T LoadAssetByTypeName<T>() where T : Object
        {
            var assetName = typeof(T).Name;
            return LoadAssetByName<T>(assetName);
        }

        public Object LoadAssetByName(string resourceName)
        {
            throw new NotImplementedException();
        }

        public T LoadAssetByName<T>(string resourceName) where T : Object
        {
            return LoadEditorAsset<T>(resourceName);
        }

        public List<T> LoadPrefabAssets<T>() where T : Component
        {
            var assets = new List<T>();
            var gameObjects = LoadAssets<GameObject>();
            foreach (var gameObject in gameObjects)
            {
                var component = gameObject.GetComponent<T>();
                if (component)
                    assets.Add(component);
            }
            return assets;
        }

        public T LoadAsset<T>(string assetName) where T : Object
        {
            GameProfiler.BeginSample(loadSimulationAsset);
            var resultAsset = LoadAssetByName<T>(assetName);
            GameProfiler.EndSample();
            return resultAsset;

        }

        public T LoadPrefabAsset<T>() where T : Component
        {
            var components = LoadPrefabAssets<T>();
            return components.FirstOrDefault();
        }


        public IEnumerator LoadAllAssets<T>(Action<List<T>> action) where T : Object
        {
            //todo
            var targetType = typeof(T);
            if (!_assets.ContainsKey(targetType))
            {
                action(new List<T>());
                yield break;
            }
            var assets = _assets[targetType];
            action(assets.OfType<T>().ToList());
        }

        public IEnumerator LoadAssetsAsync(Type type, Action<List<Object>> callback)
        {

            if (!_assets.ContainsKey(type))
            {
                callback(new List<Object>());
                yield break;
            }
            var assets = _assets[type];
            callback(assets);
        }

        #region async

        public IEnumerator LoadAssetByNameAsync<T>(string assetName, Action<T> action)
            where T : Object
        {
            var asset = LoadAssetByName<T>(assetName);
            if (action != null)
                action(asset);
            yield break;
        }

        public IEnumerator LoadPrefabAssetAsync<T>(string assetName, Action<T> callback) where T : Component
        {
            GameObject asset = null;
            yield return LoadAssetByNameAsync<GameObject>(assetName, x => asset = x);
            if (!asset)
                callback(null);
            var component = asset.GetComponent<T>();
            callback(component);
        }


        public IEnumerator LoadAssetAsync<T>(Action<T> action)
            where T : Object
        {
            var asset = LoadAsset<T>();
            action(asset);
            yield break;
        }

        public IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback)
            where T : Object
        {
            var assets = LoadAssets<T>();
            callback(assets);
            yield break;
        }

        public IEnumerator LoadPrefabAssetsAsync<T>(Action<List<T>> callback)
            where T : Component
        {
            var prefabs = LoadPrefabAssets<T>();
            callback(prefabs);
            yield break;
        }

        public IEnumerator LoadPrefabAssetAsync<T>(Action<T> action)
            where T : Component
        {
            var prefab = LoadPrefabAsset<T>();
            action(prefab);
            yield break;
        }

        public IEnumerator LoadAssetByTypeNameAsync<T>(Action<T> callback) where T : Component
        {
            var name = typeof(T).Name;
            //var assetPath = _assetPaths.FirstOrDefault(x => x.EndsWith(name, true, CultureInfo.InvariantCulture));
            var asset = LoadAssetByName<GameObject>(name);
            var component = asset.GetComponent<T>();
            if (asset)
            {
                callback(component);
            }
            yield break;
        }

        private void LoadAssetsInternal()
        {

            foreach (var assetPath in _assetPaths)
            {

                var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                var gameObject = asset as GameObject;
                if (gameObject != null)
                {
                    var components = gameObject.GetComponents(typeof(Component));
                    for (int i = 0; i < components.Length; i++)
                    {
                        AddAsset(components[i]);
                    }

                    gameObject.SetActive(false);
                    ;
                }
                AddAsset(asset);
                if (asset is Texture2D)
                {
                    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
                    AddAsset(sprite);
                }
            }
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

            LoadAssetsInternal();
        }

        private T GetCacheAsset<T>(string resourceName) where T : Object
        {
            if (string.IsNullOrEmpty(resourceName)) return null;

            if (!_namedAssets.ContainsKey(resourceName))
                return null;
            var namedItems = _namedAssets[resourceName];
            var targetType = typeof(T);

            if (_componentType.IsAssignableFrom(targetType) && namedItems.ContainsKey(_gameObjectType))
            {
                var targetObject = namedItems[_gameObjectType] as GameObject;
                return targetObject ? targetObject.GetComponent<T>() : null;
            }

            if (!namedItems.ContainsKey(targetType))
            {
                var foundType = namedItems.FirstOrDefault(x => targetType.IsAssignableFrom(x.Key)).Key;
                if (foundType == null) return null;
                return namedItems[foundType] as T;
            }
            var asset = namedItems[targetType];
            return asset as T;
        }

        private T LoadEditorAsset<T>(string assetName) where T : Object
        {

            if (_assetsMap.ContainsKey(assetName) == false)
            {
                GameLog.LogErrorFormat("Asset bundle [{0}] operation LoadAsset: [{1}] in NULL", _bundleName, assetName);
                return null;
            }

            var targetType = typeof(T);
            var isComponent = _componentType.IsAssignableFrom(targetType);
            var resultAsset = GetCacheAsset<T>(assetName);

            if (resultAsset == null)
            {

                var assets = _assetsMap[assetName];
                for (int i = 0; i < assets.Count; i++)
                {

                    var path = assets[i];
                    var asset = isComponent
                        ? AssetDatabase.LoadAssetAtPath<GameObject>(path) as Object
                        : AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset == null)
                    {
                        var loadedResource = AssetDatabase.LoadAssetAtPath<Object>(path);
                        asset = loadedResource as T;
                    }
                    if (asset == null)
                    {
                        continue;
                    }

                    //add already loaded asset to cache
                    AddAsset(asset);

                    var gameObject = asset as GameObject;
                    if (isComponent && gameObject != null)
                    {
                        var component = gameObject.GetComponent<T>();
                        if (component == false) continue;
                        resultAsset = component;
                        AddAsset(component);
                    }

                    var targetTypeAsset = asset as T;
                    if (targetTypeAsset != null)
                    {
                        resultAsset = targetTypeAsset;
                    }
                }

                if (resultAsset == null)
                {
                    GameLog.LogErrorFormat("Requested resource with name: {0} from bundle {1} is NULL", assetName,
                        _bundleName);
                    return null;
                }
            }

            if (resultAsset is GameObject || isComponent)
            {
                var component = resultAsset as Component;
                var targetObject = resultAsset as GameObject;
                var go = component != null ? component.gameObject : targetObject;
                var position = go.transform.position;
                var rotation = go.transform.rotation;
                var resultGameObject = ObjectPool.Scripts.ObjectPool.Spawn(go, position, rotation, false);
                resultAsset = isComponent ? resultGameObject.GetComponent<T>() : resultGameObject as T;
            }
            else
            {
                var bufferResult = _useInstances ? Object.Instantiate(resultAsset) : resultAsset;
                resultAsset = bufferResult ? bufferResult : resultAsset;
            }

            if (resultAsset == null)
            {
                GameLog.LogErrorFormat("Requested resource with name: {0} from bundle {1} is NULL", assetName, _bundleName);
            }
            return resultAsset;
        }

        private void AddAsset(Object asset)
        {

            if (asset == null) return;

            var assetName = asset.name;
            var type = asset.GetType();
            if (!_assets.ContainsKey(type))
            {
                _assets[type] = new List<Object>();
            }
            _assets[type].Add(asset);
            if (!_namedAssets.ContainsKey(assetName))
            {
                _namedAssets[assetName] = new Dictionary<Type, Object>();
            }
            _namedAssets[assetName][type] = asset;
        }

        #endregion
#endif

#if !UNITY_EDITOR

        //load Unity asset Objecs
        public T LoadAsset<T>() where T : Object { throw new NotImplementedException(); }
        public List<T> LoadAssets<T>() where T : Object { throw new NotImplementedException(); }
        public T LoadAssetByTypeName<T>() where T : Object { throw new NotImplementedException(); }
        public Object LoadAssetByName(string assetName) { throw new NotImplementedException(); }
        public T LoadAsset<T>(string assetName) where T : Object { throw new NotImplementedException(); }
    
        //load prefabs
        public T LoadPrefabAsset<T>() where T : Component { throw new NotImplementedException(); }
        public List<T> LoadPrefabAssets<T>() where T : Component { throw new NotImplementedException(); }
        public IEnumerator CreateAssetProvider<T>(Action<T> action) where T : Object { throw new NotImplementedException(); }
        public IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback)
            where T : Object
        { throw new NotImplementedException(); }
        public IEnumerator LoadPrefabAssetsAsync<T>(Action<List<T>> callback)
            where T : Component
        { throw new NotImplementedException(); }
        
        public IEnumerator LoadAssetsAsync(Type type, Action<List<Object>> callback) { throw new NotImplementedException(); }

        public IEnumerator LoadAssetAsync<T>(Action<T> action)  where T : Object { throw new NotImplementedException(); }

        public IEnumerator LoadPrefabAssetAsync<T>(Action<T> action) where T : Component { throw new NotImplementedException(); }

        public IEnumerator LoadAssetByTypeNameAsync<T>(Action<T> callback) where T : Component { throw new NotImplementedException(); }

        public IEnumerator LoadAssetByNameAsync<T>(string assetName, Action<T> action)where T : Object {
            throw new NotImplementedException();
        }
    
        public IEnumerator LoadPrefabAssetAsync<T>(string name,Action<T> callback) where T : Component{
            throw new NotImplementedException();
        }
#endif

    }
}
