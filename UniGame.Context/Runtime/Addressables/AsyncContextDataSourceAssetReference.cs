namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using Abstract;
    using Context.Runtime.Interfaces;
    using Scriptable;
    using UnityEngine.AddressableAssets;

    [Serializable]    
    public class AsyncContextDataSourceAssetReference : AssetReferenceT<AsyncContextDataSource> , IDisposable
    {
        public AsyncContextDataSourceAssetReference(string guid) : 
            base(guid) {}

        private void ReleaseUnmanagedResources()
        {
            if(Asset is IDisposable disposable)
                disposable.Dispose();
            ReleaseAsset();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~AsyncContextDataSourceAssetReference()
        {
            ReleaseUnmanagedResources();
        }
    }
}
