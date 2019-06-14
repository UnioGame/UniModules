namespace UniGreenModules.GBG.UI.Runtime.Interfaces
{
    using UnityEngine.AddressableAssets;

    public interface IUiScreenMapData
    {

        AssetReference FindScreen<TModel>();

    }
}
