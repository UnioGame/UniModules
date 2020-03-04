namespace Taktika.MVVM.Runtime.Settings
{
    using System;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using UniGreenModules.UniGame.AddressableTools.Runtime.Attributes;
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