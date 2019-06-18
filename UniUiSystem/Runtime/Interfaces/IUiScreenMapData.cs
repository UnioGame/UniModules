namespace UniGreenModules.UniUiSystem.Runtime.Interfaces
{
    using UnityEngine.AddressableAssets;

    public interface IUiScreenMapData
    {

        AssetReference FindScreen<TModel>();

    }
}
