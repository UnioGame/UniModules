namespace UniGreenModules.UniUiSystem.Runtime.Data
{
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = "UniGame/UI Manager/Screen Map",fileName = "UiScreenMapData")]
    public class UiScreenMapData : ScriptableObject , IUiScreenMapData
    {

        public List<ScreenItemData> screens = new List<ScreenItemData>();
        
        public AssetReference FindScreen<TModel>() => throw new System.NotImplementedException();
        
    }
}
