using System.Collections.Generic;
using UnityEngine;

namespace AssetBundlesModule {

    public interface IAssetsBundleLoader {

        AssetBundleManifest AssetBundleManifest { get; }

        IAssetBundleRequest GetAssetBundleRequest(string assetBundleName,AssetBundleSourceType sourceType,bool loadDependencies = true);

        List<string> GetBundleDependencies(string assetBundleName);

    }

}