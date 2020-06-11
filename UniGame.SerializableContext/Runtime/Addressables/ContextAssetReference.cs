namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using AssetTypes;
    using UnityEngine.AddressableAssets;

    
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
    #endif
    [Serializable]   
    public class ContextAssetReference : DisposableAssetReference<ContextAsset>
    {
        public ContextAssetReference(string guid) : base(guid) {}
    }
}
