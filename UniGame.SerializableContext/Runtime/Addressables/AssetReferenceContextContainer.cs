namespace UniModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AssetTypes;
    using UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies;

    [Serializable]   
    public class AssetReferenceContextContainer : DisposableAssetReference<ContextContainerAsset>
    {
        public AssetReferenceContextContainer(string guid) : base(guid) {}
    }
}
