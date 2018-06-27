using System.Collections.Generic;
using UnityEngine;

namespace AssetBundlesModule {

    public interface IAssetsBundleLoader {

        AssetBundleManifest AssetBundleManifest { get; }

        IAssetBundleRequest GetAssetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType,bool loadDependencies = true);

        //void LoadDependencies(string assetBundleName, BundleResourceType resourceType);

        void UnloadDependencies(string assetBundleName, bool forceUnload = false);
        void UnloadAssetBundleInternal(string assetBundleName, bool forceUnload = false);

        List<string> GetBundleDependencies(string assetBundleName);

    }

}