namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using AssetTypes;
    
    [Serializable]   
    public class AssetReferenceContextContainer : DisposableAssetReference<ContextContainerAsset>
    {
        public AssetReferenceContextContainer(string guid) : base(guid) {}
    }
}
