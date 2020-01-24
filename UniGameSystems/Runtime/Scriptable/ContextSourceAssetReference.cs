namespace UniGreenModules.UniGameSystems.Runtime.Scriptable
{
    using System;
    using UnityEngine.AddressableAssets;

    [Serializable]    
    public class ContextSourceAssetReference : AssetReferenceT<AsyncContextDataSource>
    {
        public ContextSourceAssetReference(string guid) : base(guid) {}
    }
}
