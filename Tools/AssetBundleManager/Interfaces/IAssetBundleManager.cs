using System;
using System.Collections;
using System.Collections.Generic;
using AssetBundlesModule;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IAssetBundleManager 
{
    AssetBundleManifest AssetBundleManifest { get; }

    void UnloadAssetBundle(string assetBundleName, bool force = false);

    void UnloadAllAssets(bool force = false);

    IAssetBundleResource GetAssetBundleResource(string assetBundleResource);

    IEnumerator GetAssetBundleResourceAsync(string assetBundleName, AssetBundleSourceType sourceType,
                                            Action<IAssetBundleResource> resourceAction);

    #region async operations

    IEnumerator LoadAssetsAsync<T>(AssetBundleSourceType sourceType,Action<List<T>> callback) where T : Object;

    IEnumerator LoadAssetAsync<T>(string name, AssetBundleSourceType sourceType, Action<T> callback)
        where T : Object;

    IEnumerator LoadAssetsAsync(Type type, string bundleName, AssetBundleSourceType sourceType, Action<List<Object>> callback);

    IEnumerator LoadAssetsAsync<T>(string name, AssetBundleSourceType sourceType, Action<List<T>> callback) where T : Object;

    IEnumerator LoadAssetAsync<T>(string assetBundleName, string assetName, AssetBundleSourceType sourceType, Action<T> callback) where T : Object;

    #endregion

    #region sync operations

    T LoadAsset<T>(string assetBundleName) where T : Object;

    T LoadAsset<T>(string assetBundleName,string assetName) where T : Object;

    List<T> LoadAssets<T>(string assetBundleName) where T : Object;

    #endregion

}