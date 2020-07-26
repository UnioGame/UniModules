namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AssetTypes;
    using UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies;
    using UniGreenModules.UniGame.SerializableContext.Runtime.Addressables;

#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
    #endif
    [Serializable]   
    public class ContextAssetReference : AssetReferenceDataSource<ContextAsset>
    {
        public ContextAssetReference(string guid) : base(guid) {}
    }
}
