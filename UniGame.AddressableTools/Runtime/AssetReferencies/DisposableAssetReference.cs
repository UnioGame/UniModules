using UnityEngine.AddressableAssets;

namespace UniGreenModules.UniGame.AddressableTools.Runtime.AssetReferencies
{
    using System;
    using Object = UnityEngine.Object;

    /// <summary>
    /// disposable asset reference handler
    /// </summary>
    /// <typeparam name="TAsset">target asset type</typeparam>
    [Serializable]
#if ODIN_INSPECTOR
    [Sirenix.OdinInspector.DontApplyToListElements]
    [Sirenix.OdinInspector.DrawWithUnity]
#endif
    public class DisposableAssetReference<TAsset> : AssetReferenceT<TAsset> , IDisposable
        where TAsset : Object 
    {
        public DisposableAssetReference(string guid) : base(guid) {}

        private void ReleaseUnmanagedResources()
        {
            if(Asset is IDisposable disposable)
                disposable?.Dispose();
            ReleaseAsset();
        }

        public void Dispose()
        {
            //ReleaseUnmanagedResources();
            //GC.SuppressFinalize(this);
        }

        // ~DisposableAssetReference()
        // {
        //     ReleaseUnmanagedResources();
        // }
    }
}
