namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AssetTypes;
    using UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
    #endif
    [Serializable]   
    public class ContextAssetReference : DisposableAssetReference<ContextAsset>
    {
        public ContextAssetReference(string guid) : base(guid) {}
    }
}
