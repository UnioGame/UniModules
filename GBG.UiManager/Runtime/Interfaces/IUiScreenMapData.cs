namespace UniGreenModules.GBG.UiManager.Runtime.Interfaces
{
    using UnityEngine.AddressableAssets;

    public interface IUiScreenMapData
    {

        AssetReference FindScreen<TModel>();

    }
}
