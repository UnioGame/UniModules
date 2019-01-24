using System.Collections.Generic;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;
using UniModule.UnityTools.ObjectPool.Scripts;

namespace UniModule.UnityTools.AssetBundleManager
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
