using System.Collections.Generic;
using UniModule.UnityTools.AssetBundleManager.Interfaces;

namespace UniModule.UnityTools.AssetBundleManager.LoaderExecutors {

    public interface IAssetBundleAggregateRequest : IAssetBundleRequest
    {

        void Initialize(IAssetBundleRequest bundleRequest,
                                        List<IAssetBundleRequest> dependencies);

        List<IAssetBundleRequest> Requests { get; }

    }

}