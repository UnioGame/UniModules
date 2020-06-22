namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using AssetTypes;
    
    [Serializable]   
    public class ContextContainerAssetReference : DisposableAssetReference<ContextContainerAsset>
    {
        public ContextContainerAssetReference(string guid) : base(guid) {}
    }
}
