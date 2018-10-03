using System.Collections.Generic;
using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;
using Assets.Tools.UnityTools.ObjectPool.Scripts;

namespace Assets.Tools.UnityTools.AssetBundleManager
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
