namespace UniGreenModules.AssetBundleManager.Runtime.LoaderExecutors {
    using System.Collections.Generic;
    using Interfaces;

    public interface IAssetBundleAggregateRequest : IAssetBundleRequest
    {

        void Initialize(IAssetBundleRequest bundleRequest,
                                        List<IAssetBundleRequest> dependencies);

        List<IAssetBundleRequest> Requests { get; }

    }

}