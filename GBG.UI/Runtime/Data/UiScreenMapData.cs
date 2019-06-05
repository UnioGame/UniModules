using UnityEngine;

namespace UniGreenModules.GBG.UiManager.Runtime.Configuration
{
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine.AddressableAssets;

    [CreateAssetMenu(menuName = "GBG/UI Manager/Screen Map",fileName = "UiScreenMapData")]
    public class UiScreenMapData : ScriptableObject , IUiScreenMapData
    {

        public List<ScreenItemData> screens = new List<ScreenItemData>();
        
        public AssetReference FindScreen<TModel>() => throw new System.NotImplementedException();
        
    }
}
