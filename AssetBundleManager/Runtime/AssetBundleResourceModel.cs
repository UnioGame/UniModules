namespace UniGreenModules.AssetBundleManager.Runtime
{
    using System.Collections.Generic;
    using AssetBundleResources;
    using UniCore.Runtime.ObjectPool.Runtime.Interfaces;

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
