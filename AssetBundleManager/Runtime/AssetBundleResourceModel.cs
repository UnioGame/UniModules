using System.Collections.Generic;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager
{
    using UniGreenModules.UniCore.Runtime.ObjectPool.Interfaces;

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
