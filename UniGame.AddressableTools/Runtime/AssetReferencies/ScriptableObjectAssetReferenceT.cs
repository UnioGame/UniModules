using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;

    public class ScriptableObjectAssetReferenceT<T> : AssetReferenceT<T>, IDisposable
        where T : ScriptableObject
    {
        public ScriptableObjectAssetReferenceT(string guid) : base(guid)
        {
        }

        private void ReleaseUnmanagedResources()
        {
            if(Asset is IDisposable disposable)
                disposable?.Dispose();
            ReleaseAsset();
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~ScriptableObjectAssetReferenceT()
        {
            ReleaseUnmanagedResources();
        }
    }
}
