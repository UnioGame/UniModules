namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using AddressableTools.Runtime.AssetReferencies;
    using UniModules.UniGame.Context.Runtime.Abstract;

    [Serializable]    
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
#endif
    public class AsyncContextDataSourceAssetReference : DisposableAssetReference<AsyncContextDataSource> 
    {
        public AsyncContextDataSourceAssetReference(string guid) : base(guid) {}
        
    }
}
