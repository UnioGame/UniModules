using System.Collections.ObjectModel;
using Assets.Tools.UnityTools.AssetBundleManager.AssetBundleResources;

namespace Assets.Tools.UnityTools.AssetBundleManager.Interfaces
{
    public interface IAssetBundleResourceMap {

        ReadOnlyCollection<string> GetAllNames { get; }

        bool Add(AssetBundleResourceModel resourceModel);
        bool Unload(string assetBundleName,bool force, bool forceUnloadMode);

        IAssetBundleResource Get(string bundleName);

    }
}