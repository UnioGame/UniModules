using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace UniModule.UnityTools.AssetBundleManager.AssetBundleResources
{
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

    public interface IAssetBundleResource : IPoolable
    {

        string BundleName { get; }
        string[] AllAssetsNames { get; }
        int ReferencedCount { get; set; }

        IDictionary<string, Object> CachedAssets { get; }

        IEnumerator LoadAssetAsync<T>(Action<T> callback) where T : Object;

        IEnumerator LoadAssetAsync<T>(string assetName, Action<T> callback) where T : Object;

        IEnumerator LoadAllAssetsAsync<T>(Action<List<T>> callback) where T : Object;

        IEnumerator LoadAllAssetsAsync(Type type, Action<List<Object>> callback);

        IEnumerator LoadAssetWithSubAssetsAsync(string name, Action<List<Object>> callback);

        Object LoadAsset(string assetName);
        T LoadAsset<T>() where T : Object;
        T LoadAsset<T>(string assetName) where T : Object;

        List<T> LoadAssets<T>()
            where T : Object;

        List<Object> LoadAssetWithSubAssets(string name);

        T LoadAssetByTypeName<T>() where T : Object;

        bool Unload(bool forceUnload, bool forceMode);

        bool TryUnload(bool unloadForceMode);


    }
}