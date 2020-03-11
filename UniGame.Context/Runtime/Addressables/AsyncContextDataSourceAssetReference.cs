namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using Context.Runtime.Interfaces;

    [Serializable]    
    public class AsyncContextDataSourceAssetReference : DisposableAssetReference<AsyncContextDataSource> 
    {
        public AsyncContextDataSourceAssetReference(string guid) : base(guid) {}
        
    }
}
