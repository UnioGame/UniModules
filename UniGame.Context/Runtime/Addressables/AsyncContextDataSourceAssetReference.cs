namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UniModules.UniGame.Context.Runtime.Abstract;

    [Serializable]
    public class AsyncContextDataSourceAssetReference : DisposableAssetReference<AsyncContextDataSource> 
    {
        public AsyncContextDataSourceAssetReference(string guid) : base(guid) {}
        
    }
}
