using System.Collections.ObjectModel;
using UniModule.UnityTools.AssetBundleManager.AssetBundleResources;

namespace UniModule.UnityTools.AssetBundleManager.Interfaces
{
    public interface IAssetBundleResourceMap {

        ReadOnlyCollection<string> GetAllNames { get; }

        bool Add(AssetBundleResourceModel resourceModel);
        bool Unload(string assetBundleName,bool force, bool forceUnloadMode);

        IAssetBundleResource Get(string bundleName);

    }
}