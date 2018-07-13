using System;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace AssetBundlesModule {

    public interface IAssetBundleResource : IDisposable {
        
        string BundleName { get; }
        string[] AllAssetsNames { get; }
        int ReferencedCount { get; set; }

        IEnumerable<Object> CachedObjects { get; }
        
        IEnumerator LoadAssetAsync<T>(Action<T> callback) where T : Object;

        IEnumerator LoadAssetAsync<T>(string assetName, Action<T> callback)where T : Object;

        IEnumerator LoadAllAssetsAsync<T>(Action<List<T>> callback)where T : Object;

        IEnumerator LoadAllAssetsAsync(Type type,Action<List<Object>> callback);


        Object LoadAsset(string assetName);
        T LoadAsset<T>() where T : Object;
        T LoadAsset<T>(string assetName) where T : Object;

        List<T> LoadAssets<T>()
            where T : Object;

        T LoadAssetByTypeName<T>() where T : Object;

        //unload bundle resource
        bool Unload(bool forceUnload);
    }
}