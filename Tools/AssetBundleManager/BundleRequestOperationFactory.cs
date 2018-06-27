using System.Collections.Generic;
using Assets.Tools.Utils;
using UnityEngine;

namespace AssetBundlesModule
{
    public class BundleRequestOperationFactory : IBundleRequestFactory
    {
        private readonly string _manifestName;
        private readonly bool _asyncDependencies;
        private readonly string _assetsLocation;
        
        protected static string _wwwTemplate = "file://{0}";//"{0}/";//
        

        public BundleRequestOperationFactory(string manifestName, bool asyncDependencies) {

            _manifestName = manifestName;
            _asyncDependencies = asyncDependencies;
            _assetsLocation = string.Format("{0}/{1}/", Application.streamingAssetsPath, _manifestName);

        }

        public IAssetBundleRequest Create(string bundleName, AssetBundleSourceType sourceType) {
            return CreateOperation(bundleName, sourceType);
        }

        public IAssetBundleRequest Create(IAssetBundleRequest targetBundle, 
            List<IAssetBundleRequest> dependencies, AssetBundleSourceType sourceType) {

            AssetBundleWithDependenciesBaseRequest request = null;
            switch (sourceType) {
                case AssetBundleSourceType.LocalFile:
                case AssetBundleSourceType.Simulation:
                    request = ClassPool.Spawn<AssetBundleWithDependenciesSequentRequest>();
                    break;
                default:
                {
                    if (_asyncDependencies) {
                        request = ClassPool.Spawn<AssetBundleWithRoutineDependenciesRequest>();
                    }
                    else {
                        request = ClassPool.Spawn<AssetBundleWithDependenciesSequentRequest>();
                    }
                    break;
                }
            }

            request.Initialize(targetBundle,dependencies);
            return request;

        }

        private IAssetBundleRequest CreateOperation(string assetBundleName, AssetBundleSourceType sourceType) {
            
            var bundleResource = GetAssetsBundlesPath(assetBundleName);

            //var url = GetDownloadUrl(bundleResource);

            var requestOperation = CreateRequest(sourceType);

            requestOperation.Initialize(assetBundleName,bundleResource);

            return requestOperation;
        }


        public string GetAssetsBundlesPath(string assetBundleName)
        {
            return GetStreamingAssetsPathWWW() + assetBundleName;
        }

        public string GetStreamingAssetsPathWWW()
        {
            return _assetsLocation;
            if (Application.isMobilePlatform || Application.isConsolePlatform)
            {
                return _assetsLocation;
            }
            return string.Format(_wwwTemplate, _assetsLocation); // Use the build output folder directly.
            //else if (Application.isMobilePlatform || Application.isConsolePlatform)
            //    return Application.streamingAssetsPath;
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
