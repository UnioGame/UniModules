namespace UniGreenModules.UniGame.SerializableContext.Runtime.Addressables
{
    using System;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

#if ODIN_INSPECTOR
    using Sirenix.OdinInspector;
#endif
    
#if ODIN_INSPECTOR
    [DontApplyToListElements]
#endif
    
    [Serializable]
    public class ScriptableObjectAssetReference : AssetReferenceT<ScriptableObject> , IDisposable
    {
        public ScriptableObjectAssetReference(string guid) : base(guid) {}
        
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

        ~ScriptableObjectAssetReference()
        {
            ReleaseUnmanagedResources();
        }
    }
}
