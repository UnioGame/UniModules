namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using Scriptable;
    using UnityEngine.AddressableAssets;

    [Serializable]    
    public class ContextDataSourceAssetReference : AssetReferenceT<AsyncContextDataSource>
    {
        public ContextDataSourceAssetReference(string guid) : base(guid) {}
    }
}
