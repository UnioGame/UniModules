using System.Collections.Generic;

namespace AssetBundlesModule {

    public interface IAssetBundleAggregateRequest : IAssetBundleRequest
    {

        void Initialize(IAssetBundleRequest bundleRequest,
                                        List<IAssetBundleRequest> dependencies);

        List<IAssetBundleRequest> Requests { get; }

    }

}