namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AssetTypes;
    using UnityEngine.AddressableAssets;

    [Serializable]    
    public class ContextAssetReference : AssetReferenceT<ContextAsset>
    {
        public ContextAssetReference(string guid) : base(guid) {}
    }
}
