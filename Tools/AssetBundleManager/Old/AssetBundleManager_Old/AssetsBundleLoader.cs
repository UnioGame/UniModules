using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.ProfilerTools;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AssetBundlesModule_Old{
    public static class AssetsBundleLoader
    {
        private const string ManifestName = "AssetBundles";

        #region static data
        
        private static Dictionary<string, LoadedAssetBundle> _loadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        private static Dictionary<string, AssetBundleCreateRequest> _downloadingRequest = new Dictionary<string, AssetBundleCreateRequest>();
        private static HashSet<string> _loadingBundles = new HashSet<string>();
        private static Dictionary<string, string> _downloadingErrors = new Dictionary<string, string>();
        private static Dictionary<string, string[]> _dependencies = new Dictionary<string, string[]>();
        private static string _streamingAssetPath;

        public static string AssetsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_streamingAssetPath))
                {
                    _streamingAssetPath = GetStreamingAssetsPathWWW();
                }
                return _streamingAssetPath;
            }
        }

        public static AssetBundleManifest AssetBundleManifest { get; private set; }

        #endregion

        #region public methods

        // Get loaded AssetResource, only return vaild object when all the dependencies are downloaded successfully.

        public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName)
        {
            string error;
            if (_downloadingErrors.TryGetValue(assetBundleName, out error))
            {
                Debug.LogErrorFormat("LoadedAssetBundle Error {0}", error);
                return null;
            }
            var bundle = GetCacheAssetBundle(assetBundleName);
            if (bundle == null)
            {
                Debug.LogErrorFormat("Asset bundle {0} loading result NULL", assetBundleName);
                return null;
            }

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies;
            if (!_dependencies.TryGetValue(assetBundleName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                if (_downloadingErrors.TryGetValue(assetBundleName, out error))
                    return bundle;
                // Wait all the dependent assetBundles being loaded.
                LoadedAssetBundle dependentBundle;
                _loadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }
            return bundle;
        }

        public static string GetError(string assetName)
        {
            string error;
            _downloadingErrors.TryGetValue(assetName, out error);
            return error;
        }

        // Load AssetResource and its dependencies.
        public static IEnumerator LoadAssetBundleAsync(string assetBundleName, bool loadDependencies = true)
        {
            if (AssetBundleManifest == null)
            {
                LoadManifest(ManifestName);
            }

            var id = GameProfiler.BeginWatch(string.Format("Load Bundle & Depend's Async: {0}", assetBundleName));

            // SetHeaders if the assetBundle has already been processed.
            yield return LoadAssetBundleInternalAsync(assetBundleName);

            //load all Dependencies id needed
            if (loadDependencies) yield return LoadDependenciesAsync(assetBundleName);

            GameProfiler.StopWatch(id);

            yield break;
        }


        public static LoadedAssetBundle LoadAssetBundle(string assetBundleName, bool loadDependencies = true) {

            if (AssetBundleManifest == null)
            {
                LoadManifest(ManifestName);
            }
            var cachedBundle = GetCacheAssetBundle(assetBundleName);
            if (cachedBundle != null)
                return cachedBundle;

            var id = GameProfiler.BeginWatch(string.Format("Load Bundle & Depend's: {0}", assetBundleName));

            var resultBundle = LoadAssetBundleInternal(assetBundleName);

            //load all Dependencies id needed
            if (loadDependencies)
                LoadDependencies(assetBundleName);

            GameProfiler.StopWatch(id);

            return resultBundle;
        }


        // Where we get all the dependencies and load them all.
        public static IEnumerator LoadDependenciesAsync(string assetBundleName)
        {
            var id = GameProfiler.BeginWatch(string.Format("Load DependenciesAsync [{0}]: ", assetBundleName));

            var dependencies = GetBundleDependencies(assetBundleName);

            LogDependencies(dependencies, assetBundleName);

            for (var i = 0; i < dependencies.Length; i++)
                yield return LoadAssetBundleInternalAsync(dependencies[i]);

            GameProfiler.StopWatch(id);
        }

        // Where we get all the dependencies and load them all.
        public static void LoadDependencies(string assetBundleName)
        {
            var id = GameProfiler.BeginWatch(string.Format("Load Dependencies [{0}]: ", assetBundleName));
            
            var dependencies = GetBundleDependencies(assetBundleName);

            LogDependencies(dependencies, assetBundleName);

            for (var i = 0; i < dependencies.Length; i++) {
                var dependence = dependencies[i];
                if(string.Equals(assetBundleName, dependence,StringComparison.OrdinalIgnoreCase))
                    continue;
                LoadAssetBundleInternal(dependencies[i]);
            }

            GameProfiler.StopWatch(id);
        }


        public static void UnloadDependencies(string assetBundleName, bool forceUnload = false)
        {
            string[] dependencies;
            if (!_dependencies.TryGetValue(assetBundleName, out dependencies))
                return;
            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency, forceUnload);
            }
            _dependencies.Remove(assetBundleName);
        }

        public static void UnloadAssetBundleInternal(string assetBundleName, bool forceUnload = false)
        {
            var bundle = GetLoadedAssetBundle(assetBundleName);
            if (bundle == null)
                return;
            if (--bundle.ReferencedCount != 0) return;
            bundle.AssetBundle.Unload(forceUnload);
            _loadedAssetBundles.Remove(assetBundleName);
            GameLog.LogFormat("{0} has been unloaded successfully FORCE mode = {1}", assetBundleName, forceUnload);
        }

        #endregion

        #region private methods

        private static LoadedAssetBundle LoadAssetBundleInternal(string assetBundleName) {

            //If asset bundle already requested with www wait result
            var wwwAwaiter = WaitBundleWWW(assetBundleName);
            wwwAwaiter.WaitCoroutine();

            var resultBundle = GetCacheAssetBundle(assetBundleName);
            if (resultBundle != null) return resultBundle;

            var id = GameProfiler.BeginWatch(string.Format("Load Asset Bundle Internal :[{0}]",assetBundleName));
            GameProfiler.BeginSample("Load Asset Bundle Internal");

            var assetBundlePath = GetAssetsBundlesPath(assetBundleName);

            var bundle = AssetBundle.LoadFromFile(assetBundlePath);
            if (bundle == null)
            {
                GameLog.LogErrorFormat("Asset bundle {0} is NULL", assetBundleName);
            }

            AddLoadedAssetBundle(assetBundleName, bundle);
            resultBundle = GetCacheAssetBundle(assetBundleName);

            GameProfiler.EndSample();
            GameProfiler.StopWatch(id);
            
            return resultBundle;
        }

        public static void LoadManifest(string assetBundleName = ManifestName)
        {
            LoadAssetBundleInternal(assetBundleName);
            if (!_loadedAssetBundles.ContainsKey(assetBundleName) || AssetBundleManifest) {
                return;
            }
            AssetBundleManifest = _loadedAssetBundles[assetBundleName].AssetBundle.
                LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }

        [Conditional("LOGS_ENABLED")]
        private static void LogDependencies(string[] dependencies,string assetBundleName) {
            var abDependencies = string.Format("AB {0} dependencies ", assetBundleName);
            for (var i = 0; i < dependencies.Length; i++)
                abDependencies = string.Format("{0} : {1}", abDependencies, dependencies[i]);
            GameLog.Log(abDependencies);
        }

        private static string[] GetBundleDependencies(string assetBundleName)
        {

            if (_dependencies.ContainsKey(assetBundleName))
                return _dependencies[assetBundleName];

            // Get dependecies from the AssetBundleManifest object..
            var dependencies = AssetBundleManifest.GetAllDependencies(assetBundleName);

            // Record and load all dependencies.
            _dependencies.Add(assetBundleName, dependencies);

            return dependencies;
        }

        private static IEnumerator WaitLoading(AssetBundleCreateRequest download)
        {
            // If downloading succeeds.
            while (download != null && !download.isDone)
                yield return null;
            if (download == null)
            {
                GameLog.LogError("Asset bundle WWW is NULL");
                yield break;
            }

        }

        public static string GetAssetsBundlesPath(string assetBundleName)
        {
            return GetStreamingAssetsPathWWW() + assetBundleName;
        }


        public static string GetStreamingAssetsPath()
        {
            var assetLocation = string.Format("{0}/{1}/", Application.streamingAssetsPath, ManifestName);
            return assetLocation;
        }

        public static string GetStreamingAssetsPathWWW()
        {
            var assetLocation = GetStreamingAssetsPath();
            return assetLocation;
            //if (Application.isMobilePlatform || Application.isConsolePlatform)
            //{
            //    return assetLocation;
            //}
            //return string.Format(_wwwTemplate, assetLocation); // Use the build output folder directly.
            //else if (Application.isMobilePlatform || Application.isConsolePlatform)
            //    return Application.streamingAssetsPath;
        }

        // Where we actuall call WWW to assetBundle the assetBundle.
        private static IEnumerator LoadAssetBundleInternalAsync(string assetBundleName) {

            if (_loadingBundles.Contains(assetBundleName) &&
                !_loadedAssetBundles.ContainsKey(assetBundleName))
            {
                while (_loadingBundles.Contains(assetBundleName))
                {
                    yield return null;
                }
                yield break;
            }

            if (GetCacheAssetBundle(assetBundleName) != null)
                yield break;

            GameProfiler.BeginSample("Load Asset Bundle Internal Async");
            var id = GameProfiler.BeginWatch(string.Format("Load Asset Bundle Internal Async: [{0}]",assetBundleName));

            _loadingBundles.Add(assetBundleName);
            yield return MakeBundleRequest(assetBundleName);
            _loadingBundles.Remove(assetBundleName);

            GameProfiler.StopWatch(id);
            GameProfiler.EndSample();
        }
        
        private static IEnumerator MakeBundleRequest(string assetBundleName) {

            var assetBundlePath = GetAssetsBundlesPath(assetBundleName);
            var download = GetDownloadRequest(assetBundlePath);
            yield return WaitLoading(download);
            AddLoadedAssetBundle(assetBundleName, download.assetBundle);
            _downloadingRequest.Remove(assetBundlePath);
            
        }

        private static LoadedAssetBundle GetCacheAssetBundle(string assetBundleName)
        {
            // Already loaded.
            LoadedAssetBundle bundle;
            _loadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.ReferencedCount++;
            }
            return bundle;
        }

        private static void AddLoadedAssetBundle(string assetBundleName, AssetBundle assetBundle)
        {
            if (!assetBundle)
            {
                return;
            }
            var loadedAsset = new LoadedAssetBundle(assetBundle);
            _loadedAssetBundles.Add(assetBundleName, loadedAsset);
        }

        private static IEnumerator WaitBundleWWW(string assetBundleName) {
            AssetBundleCreateRequest request;
            if (_downloadingRequest.TryGetValue(assetBundleName, out request)) {
                yield return WaitLoading(request);
            }
        }

        private static AssetBundleCreateRequest GetDownloadRequest(string assetBundleName)
        {
            if (_downloadingRequest.ContainsKey(assetBundleName))
                return _downloadingRequest[assetBundleName];
            var url = GetDownloadUrl(assetBundleName);
            var download = AssetBundle.LoadFromFileAsync(url);
            _downloadingRequest.Add(assetBundleName, download);
            return download;
        }

        private static string GetDownloadUrl(string assetBundleName)
        {
            return assetBundleName;
        }

        #endregion
    }
}
