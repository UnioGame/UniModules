using System;
using UniGreenModules.UniGame.SerializableContext.Runtime.Scriptable;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using Context.Runtime.Interfaces;

    [Serializable]
    public class AsyncContextDataSourceAssetReference : AssetReferenceT<AsyncContextDataSource>
    {
        public AsyncContextDataSourceAssetReference(string guid) : base(guid) {}
    }
}