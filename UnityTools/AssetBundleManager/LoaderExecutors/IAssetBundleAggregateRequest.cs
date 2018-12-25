using System.Collections.Generic;
using Assets.Tools.UnityTools.AssetBundleManager.Interfaces;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors {

    public interface IAssetBundleAggregateRequest : IAssetBundleRequest
    {

        void Initialize(IAssetBundleRequest bundleRequest,
                                        List<IAssetBundleRequest> dependencies);

        List<IAssetBundleRequest> Requests { get; }

    }

}