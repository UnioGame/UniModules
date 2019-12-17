namespace UniGreenModules.AssetBundleManager.Runtime.Interfaces
{
    using System.Collections.ObjectModel;
    using AssetBundleResources;

    public interface IAssetBundleResourceMap {

        ReadOnlyCollection<string> GetAllNames { get; }

        bool Add(AssetBundleResourceModel resourceModel);
        bool Unload(string assetBundleName,bool force, bool forceUnloadMode);

        IAssetBundleResource Get(string bundleName);

    }
}