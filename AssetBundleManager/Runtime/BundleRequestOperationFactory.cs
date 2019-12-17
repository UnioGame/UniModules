namespace UniGreenModules.AssetBundleManager.Runtime
{
    using System.Collections.Generic;
    using Interfaces;
    using LoaderExecutors;
    using UniCore.Runtime.ObjectPool.Runtime;
    using UnityEngine;

    public class BundleRequestOperationFactory : IBundleRequestFactory
    {
        private readonly string _manifestName;
        private readonly bool _asyncDependencies;
        private readonly IAssetBundleResourceMap _resourceMap;
        private readonly IAssetBundlesRequestCache _requestCache;
        private readonly string _assetsLocation;
        
        protected static string _wwwTemplate = "file://{0}";//"{0}/";//
        

        public BundleRequestOperationFactory(string manifestName, 
                                             bool asyncDependencies, 
                                             IAssetBundleResourceMap resourceMap,
                                             IAssetBundlesRequestCache requestCache) {

            _manifestName = manifestName;
            _asyncDependencies = asyncDependencies;
            _resourceMap = resourceMap;
            _requestCache = requestCache;
            _assetsLocation = string.Format("{0}/{1}/", Application.streamingAssetsPath, _manifestName);

        }

        public IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType) {
            return CreateOperation(bundleName, sourceType);
        }

        public IAssetBundleRequest Create(string targetBundle, 
            List<string> dependencies, AssetBundleSourceType sourceType) {

            var aggregateRequest = CreateAggregate(sourceType);

            var targetRequest = CreateOperation(targetBundle, sourceType);

            var dependenciesReqiests = ClassPool.Spawn<List<IAssetBundleRequest>>();

            for (var i = 0; i < dependencies.Count; i++)
            {
                var request = CreateOperation(dependencies[i], sourceType);
                dependenciesReqiests.Add(request);
            }

            aggregateRequest.Initialize(targetRequest, dependenciesReqiests);
            return aggregateRequest;

        }

        private IAssetBundleAggregateRequest CreateAggregate(AssetBundleSourceType sourceType) {

            AssetBundleWithDependenciesBaseRequest request = null;
            switch (sourceType)
            {
                case AssetBundleSourceType.LocalFile:
                case AssetBundleSourceType.Simulation:
                    request = ClassPool.SpawnOrCreate(() =>
                        new AssetBundleWithDependenciesSequentRequest(_resourceMap,_requestCache));
                    break;
                default:
                {
                    if (_asyncDependencies)
                    {
                        request = ClassPool.SpawnOrCreate(() =>
                            new AssetBundleWithRoutineDependenciesRequest(_resourceMap,_requestCache));
                    }
                    else
                    {
                        request = ClassPool.SpawnOrCreate(() =>
                            new AssetBundleWithDependenciesSequentRequest(_resourceMap,_requestCache));
                    }
                    break;
                }
            }

            return request;
        }

        private IAssetBundleRequest CreateOperation(string assetBundleName, AssetBundleSourceType sourceType) {

            var request = _requestCache.Get(assetBundleName);
            if (request != null)
                return request;

            var loadedResource = _resourceMap.Get(assetBundleName);
            if (loadedResource != null) {
                request = ClassPool.SpawnOrCreate(() =>
                    new LoadedAssetBundleRequest(loadedResource));
                return request;
            }

            var bundleResource = GetAssetsBundlesPath(assetBundleName);

            request = CreateRequest(sourceType);

            request.Initialize(assetBundleName,bundleResource);

            _requestCache.Add(assetBundleName, sourceType,request);

            return request;
        }


        public string GetAssetsBundlesPath(string assetBundleName)
        {
            return GetStreamingAssetsPathWWW() + assetBundleName;
        }

        public string GetStreamingAssetsPathWWW()
        {
            return _assetsLocation;
        }


        private string GetDownloadUrl(string assetBundleName)
        {
            var url = assetBundleName;
#if UNITY_SWITCH && !UNITY_EDITOR
            url = "file:///" + url;
#endif
            return url;
        }

        private IAssetBundleRequest CreateRequest(AssetBundleSourceType sourceType) {

            IAssetBundleRequest request = null;

            switch (sourceType)
            {

                case AssetBundleSourceType.Simulation:
                    request = ClassPool.Spawn<AssetBundleSimulateRequest>();
                    break;
                case AssetBundleSourceType.AsyncLocalFile:
                    request = ClassPool.Spawn<AssetBundleLocalFileAsyncRequest>();
                    break;
                case AssetBundleSourceType.LocalFile:
                    request = ClassPool.Spawn<AssetBundleLocalFileRequest>();
                    break;
                case AssetBundleSourceType.WebRequest:
                    //todo change to WebRequest 
                    request = ClassPool.Spawn<AssetBundleWwwRequest>();
                    break;
                case AssetBundleSourceType.WWW:
                    request = ClassPool.Spawn<AssetBundleWwwRequest>();
                    break;
            }

            return request;
        }
    }
}
