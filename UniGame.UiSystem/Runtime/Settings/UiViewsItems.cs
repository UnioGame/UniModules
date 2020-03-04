namespace UniGreenModules.UniGame.UiSystem.Runtime.Settings
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UnityEngine.AddressableAssets;

    [Serializable]
    public class UiViewsItems
    {
        public string viewTag;
        
        [DrawWithUnity]
        [ShowAssetReference]
        public List<AssetReferenceGameObject> views = new List<AssetReferenceGameObject>();
    }
}