namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UniContextData.Runtime.Interfaces;
    using UnityEngine;

    [Serializable]
    public class AssetReferenceDataSource<TAsset> : AssetReferenceScriptableObject<TAsset,IAsyncContextDataSource> 
        where TAsset : ScriptableObject
    {
        public AssetReferenceDataSource(string guid) : base(guid) {}
        
    }
    
    [Serializable]
    public class AssetReferenceDataSource : AssetReferenceDataSource<ScriptableObject> 
    {
        public AssetReferenceDataSource(string guid) : base(guid) {}
        
    }
}
