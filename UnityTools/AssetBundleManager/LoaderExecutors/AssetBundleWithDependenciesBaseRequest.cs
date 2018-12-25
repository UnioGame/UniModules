using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Tools.UnityTools.AssetBundleManager.Interfaces;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.AssetBundleManager.LoaderExecutors {
    public class AssetBundleWithDependenciesBaseRequest : AssetBundleRequest, IAssetBundleAggregateRequest {

        private readonly IAssetBundleResourceMap _resourceMap;
        private readonly IAssetBundlesRequestCache _requestCache;
        protected IAssetBundleRequest _bundleRequest;
        protected List<IAssetBundleRequest> _dependencies;

        public AssetBundleWithDependenciesBaseRequest(IAssetBundleResourceMap resourceMap ,
                                                      IAssetBundlesRequestCache requestCache) :base() {
            _resourceMap = resourceMap;
            _requestCache = requestCache;

            _dependencies = new List<IAssetBundleRequest>();
            Requests = new List<IAssetBundleRequest>();
        }

        public void Initialize(IAssetBundleRequest bundleRequest,
            List<IAssetBundleRequest> dependencies) {

            OnReset();

            _bundleRequest = bundleRequest;
            _dependencies = dependencies;

            Requests.Add(bundleRequest);
            Requests.AddRange(_dependencies);

            Resource = bundleRequest.Resource;
            BundleName = bundleRequest.BundleName;

        }


        public List<IAssetBundleRequest> Requests { get; protected set; }

        protected override IEnumerator MoveNext() {
            yield break;
        }

        protected override void OnReset() {
            base.OnReset();
            if(_bundleRequest!=null)
                _requestCache.Remove(_bundleRequest.BundleName);

            if (_dependencies != null) {
                for (var i = 0; i < _dependencies.Count; i++) {
                    _requestCache.Remove(_dependencies[i].BundleName);
                }

                _dependencies.Despawn();
            }

            _bundleRequest = null;
            Requests.Clear();
        }

        protected override void OnComplete() {
            base.OnComplete();
            BundleResource = _bundleRequest.BundleResource;
            AddResource(_bundleRequest, _dependencies);

            for (var i = 0; i < Requests.Count; i++) {
                var request = Requests[i];
                AddResource(request, null);
            }

        }

        private void AddResource(IAssetBundleRequest request, List<IAssetBundleRequest> dependences) {

            var bundleModel = ClassPool.Spawn<AssetBundleResourceModel>();
            bundleModel.BundleResource = request.BundleResource;
            if (dependences != null) {
                bundleModel.Dependecies.Clear();
                bundleModel.Dependecies.AddRange(dependences.Select(x => x.BundleResource));
            }

            _resourceMap.Add(bundleModel);
        }
    }
}