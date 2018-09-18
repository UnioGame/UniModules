using System;
using System.Collections.Generic;
using Assets.Tools.Utils;

namespace AssetBundlesModule
{
    public class AssetBundleResourceModel : IPoolable {
        
        public List<IAssetBundleResource> Dependecies = new List<IAssetBundleResource>();
        public IAssetBundleResource BundleResource;

        public void Release() {
            if(Dependecies!=null)
                Dependecies.Clear();
            BundleResource = null;
        }

    }
}
