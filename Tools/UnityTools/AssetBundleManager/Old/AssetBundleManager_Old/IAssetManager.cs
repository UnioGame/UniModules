using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssetBundlesModule_Old
{
        
    public interface IAssetManager //: IAssetProvider
    {
        string AssetsUrl { get; set; }
        void UnloadAssetBundle(string assetBundleName, bool force = false);
        IAssetProvider LoadAsset(string name);

        T LoadAsset<T>(string assetBundleName, string assetName) where T : Object;

        #region async operations

        IEnumerator LoadAssetsAsync<T>(Action<List<T>> callback) where T : Object;

            IEnumerator CreateAssetProvider<T>(string name, Action<T> callback)
            where T : Object;

        IEnumerator LoadAssetsAsync(Type type, string bundleName, Action<List<Object>> callback);

        IEnumerator LoadAssetsAsync<T>(string name, Action<List<T>> callback) where T : Object;

        IEnumerator LoadAllPrefabAssetsAsync<T>(string name, Action<List<T>> callback) where T : Component;

        IEnumerator LoadPrefabAssetAsync<T>(string name, Action<T> action)
        where T : Component;

        IEnumerator LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive);

        IEnumerator LoadAssetByTypeNameAsync<T>(string name, Action<T> callback) where T : Component;

        #endregion

        void UnloadAllAssets(bool force = false);
        string GetAssetPath(string assetBundleName);
    }
}