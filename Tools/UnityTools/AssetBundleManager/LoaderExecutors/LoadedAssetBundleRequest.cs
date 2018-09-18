using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetBundlesModule
{
    public class LoadedAssetBundleRequest : AssetBundleRequest
    {

        public LoadedAssetBundleRequest(IAssetBundleResource bundleResource) : base() {

            BundleResource = bundleResource;
            BundleName = bundleResource.BundleName;

        }

    }
}
