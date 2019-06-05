using UnityEngine;

namespace UniGreenModules.GBG.UiManager.Runtime.Configuration
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class ScreenItemData
    {
        
        public string ScreenName;
        
        public AssetReference Screen;

        public long TypeId;

    }
}
