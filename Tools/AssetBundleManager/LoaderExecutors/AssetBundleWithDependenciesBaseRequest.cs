using System.Collections;
using System.Collections.Generic;
using Assets.Tools.Utils;

namespace AssetBundlesModule {
    public class AssetBundleWithDependenciesBaseRequest : AssetBundleRequest {
        protected IAssetBundleRequest _bundleRequest;
        protected List<IAssetBundleRequest> _dependencies;
        protected List<IAssetBundleRequest> _allRequests = new List<IAssetBundleRequest>();

        public void Initialize(IAssetBundleRequest bundleRequest,
            List<IAssetBundleRequest> dependencies) {

            _allRequests.Clear();

            _bundleRequest = bundleRequest;
            _dependencies = dependencies;

            _allRequests.Add(bundleRequest);
            _allRequests.AddRange(_dependencies);

            Resource = bundleRequest.Resource;
            BundleName = bundleRequest.BundleName;

        }

        protected override IEnumerator MoveNext() {
            yield break;
        }

        protected override void OnReset() {
            base.OnReset();
            _bundleRequest = null;
            _dependencies.Despawn();
            _allRequests.Clear();
        }

        protected override void OnComplete() {
            base.OnComplete();
            BundleResource = _bundleRequest.BundleResource;
        }
    }
}